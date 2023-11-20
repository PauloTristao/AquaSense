
function apagarRegistro(id, controller) {
    if (confirm('Confirma a exclusão do registro?'))
        location.href = controller + '/Delete?id=' + id;
}

function AtualizaConjuntoHabitacional(url) {
    var idConjuntoHabitacional = $("#IdConjuntoHabitacional").val();
    var nomeConjuntoModal = $("#NomeConjuntoModal").val();
    var enderecoConjuntoModal = $("#EnderecoConjuntoModal").val();
    var cnpjConjuntoModal = $("#CnpjConjuntoModal").val();
    var idUsuarioAdm = $("#IdUsuarioAdm").val();

    if (!idConjuntoHabitacional || !nomeConjuntoModal || !enderecoConjuntoModal || !cnpjConjuntoModal || !idUsuarioAdm) {
        alert("Por favor, preencha todos os campos obrigatórios.");
        return;
    }

    var data =
    {
        Model: {
            Id: idConjuntoHabitacional,
            Nome: nomeConjuntoModal,
            IdUsuarioAdm: idUsuarioAdm,
            Endereco: enderecoConjuntoModal,
            Cnpj: cnpjConjuntoModal
        },
        Operacao: "A"
    };

    $.ajax({
        url: url,
        type: "POST",
        data: data,
        success: function (data) {
            location.href = "/ConjuntoHabitacional/Index";
        },
        error: function () {
            alert("Erro ao carregar o conteúdo.");
        }
    });
};


function CriaNovoApartamento(url) {
    var nomeNovoApartamento = $("#descricaoNovoApartamento").val();
    var idConjuntoHabitacional = $("#IdConjuntoHabitacional").val();
    var idSensor = $("#sensor").val();
    var idUsuario = $("#usuario").val();

    var model =
    {
        model: {
            Descricao: nomeNovoApartamento,
            IdConjuntoHabitacional: idConjuntoHabitacional,
            IdSensor: idSensor,
            IdUsuario: idUsuario
        },
        Operacao: "I"
    }

    $.ajax({
        url: 'Apartamento/ConsultaApartamentoNome',
        type: "POST",
        data: model,
        async: false,
        success: function (data) {
            if (data == true) {
                alert("Descrição já utilizada!");
            }
            else {
                $.ajax({
                    url: url,
                    type: "POST",
                    data: model,
                    success: function (data) {
                        location.href = "/ConjuntoHabitacional/Index";
                    },
                    error: function () {
                        alert("Erro ao carregar o conteúdo.");
                    }
                });
            }
        },
        error: function () {
            alert("Erro ao carregar o conteúdo.");
        }
    });
}

function ConsultaSensoresDisponiveis(idSensor) {
    $.ajax({
        url: '../Sensor/ConsultaSensoresDisponiveis',
        type: "POST",
        async: false,
        success: function (data) {
            $("#sensor").empty();
           

            $("#sensor").append('<option value="0">Selecione</option>');
            $.each(data, function (index, item) {
                $("#sensor").append('<option value="' + item.id + '">' + item.descricao + '</option>');
            });
            if (idSensor != null && idSensor != 0) {
                $.ajax({
                    url: '../Sensor/Consulta?idSensor=' + idSensor,
                    type: "POST",
                    async: false,
                    success: function (data) {
                        $("#sensor").append('<option value="' + data.id + '">' + data.descricao + '</option>');
                        $("#sensor").val(idSensor);
                    }
                });
            }


        },
        error: function () {
            alert("Erro ao carregar o conteúdo.");
        }
    });
}

function ConsultaUsuarios() {
    $.ajax({
        url: 'Usuario/ConsultaUsuariosCombo',
        type: "POST",
        async: false,
        success: function (data) {
            $("#usuario").empty();

            $("#usuario").append('<option value="0">Selecione</option>');
            $.each(data, function (index, item) {
                $("#usuario").append('<option value="' + item.id + '">' + item.descricao + '</option>');
            });

        },
        error: function () {
            alert("Erro ao carregar o conteúdo.");
        }
    });
}

function ConsultaApartamento(url, id) {
    urlCompleta = url + "?idApartamento=" + id;
    window.location.href = urlCompleta;
}

function AtualizaApartamento(url) {
    var idApartamento = $("#IdApartamento").val();
    var descricaoApartamento = $("#DescricaoModal").val();
    var idUsuarioModal = $("#IdUsuarioModal").val();
    var idSensor = $("#IdSensor").val();
    var idConjuntoHabitacional = $("#IdConjuntoHabitacional").val();

    if (!idApartamento || !descricaoApartamento || !idUsuarioModal ) {
        alert("Por favor, preencha todos os campos obrigatórios.");
        return;
    }

    var data =
    {
        Model: {
            Id: idApartamento,
            Descricao: descricaoApartamento,
            IdConjuntoHabitacional: idConjuntoHabitacional,
            IdSensor: idSensor,
            IdUsuario: idUsuarioModal,
        },
        Operacao: "A"
    };

    $.ajax({
        url: url,
        type: "POST",
        data: data,
        success: function (data) {
            location.href = "/Apartamento/CarregaApartamento?idApartamento=" + idApartamento;
        },
        error: function () {
            alert("Erro ao carregar o conteúdo.");
        }
    });
};