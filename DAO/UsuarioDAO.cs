using AquaSense.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AquaSense.DAO
{
    public class UsuarioDAO : PadraoDAO<UsuarioViewModel>
    {
        protected override SqlParameter[] CriaParametros(UsuarioViewModel model)
        {
            object imgByte = model.ImagemEmByte;
            if (imgByte == null)
                imgByte = DBNull.Value;

            SqlParameter[] p = new SqlParameter[]
            {
                new SqlParameter("id", model.Id),
                new SqlParameter("login_usuario", model.LoginUsuario),
                new SqlParameter("nome_pessoa", model.NomePessoa),
                new SqlParameter("senha", model.Senha),
                new SqlParameter("imagem", imgByte),
                new SqlParameter("adm", model.Adm),
            };

            return p;
        }

        public override int Insert(UsuarioViewModel model)
        {
            try
            {
                using (var transacao = new System.Transactions.TransactionScope())
                {
                    model.Adm = false;
                    ConjuntoHabitacionalDAO conjuntoDao = new ConjuntoHabitacionalDAO();
                    HelperDAO.ExecutaProc("spInsert_" + Tabela, CriaParametros(model), ChaveIdentity);
                    transacao.Complete();
                }
                return model.Id;
            }
            catch (Exception erro)
            {
                throw new Exception("Não foi possível salvar o usuario" + erro.Message);
            }

        }

        public void InsertUsuarioAdm(UsuarioAdmViewModel model)
        {
            try
            {
                using (var transacao = new System.Transactions.TransactionScope())
                {
                    UsuarioViewModel usuario = model.Usuario;
                    usuario.Adm = true;
                    ConjuntoHabitacionalDAO conjuntoDao = new ConjuntoHabitacionalDAO();
                    ConjuntoHabitacionalViewModel conjunto = model.ConjuntoHabitacional;
                    conjunto.IdUsuarioAdm = HelperDAO.ExecutaProc("spInsert_" + Tabela, CriaParametros(usuario), ChaveIdentity);
                    conjuntoDao.Insert(conjunto);
                    transacao.Complete();
                }
            }
            catch (Exception erro)
            {
                throw new Exception("Não foi possível salvar o usuario" + erro.Message);
            }

        }

        public override void Update(UsuarioViewModel model)
        {
            try
            {
                using (var transacao = new System.Transactions.TransactionScope())
                {
                    HelperDAO.ExecutaProc("spUpdate_" + Tabela, CriaParametros(model));
                    ConjuntoHabitacionalDAO conjuntoDAO = new ConjuntoHabitacionalDAO();

                    transacao.Complete();
                }
            }
            catch (Exception erro)
            {
                throw new Exception("Não foi possível salvar o usuario" + erro.Message);
            }
        }

        protected override UsuarioViewModel MontaModel(DataRow registro)
        {
            UsuarioViewModel u = new UsuarioViewModel()
            {
                Id = Convert.ToInt32(registro["id_usuario"]),
                LoginUsuario = registro["login_usuario"].ToString(),
                NomePessoa = registro["nome_pessoa"].ToString(),
                Adm = Convert.ToBoolean(registro["adm"])
            };

            if (registro["senha"] != DBNull.Value)
                u.Senha = registro["senha"].ToString();

            if (registro["imagem"] != DBNull.Value)
                u.ImagemEmByte = registro["imagem"] as byte[];
            return u;
        }

        public UsuarioViewModel ConsultaAcesso(string login, string senha)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("login_usuario", login),
                new SqlParameter("senha", senha),
            };

            var tabela = HelperDAO.ExecutaProcSelect("spConsulta_Acesso", p);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }

        public List<UsuarioViewModel> ConsultaAvancadaUsuario(string login, string nomePessoa, int adm)
        {
            SqlParameter[] p = {
                new SqlParameter("login", login),
                new SqlParameter("nomePessoa", nomePessoa),
                new SqlParameter("adm", adm),
            };

            var tabela = HelperDAO.ExecutaProcSelect("spConsultaAvancadaUsuarios", p);
            var lista = new List<UsuarioViewModel>();
            foreach (DataRow dr in tabela.Rows)
                lista.Add(MontaModel(dr));

            return lista;
        }

        protected override void SetTabela()
        {
            Tabela = "Usuario";
            ChaveIdentity = true;
        }
    }
}
