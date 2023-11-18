

$(document).ready(function init() {
    const ctx = document.getElementById('myChart');
    url = "Api/RequestHistory";
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {

            var listaDeValores = data.map(function (objeto) {
                return objeto.attrValue;
            });

            var listaDeDatas = data.map(function (objeto) {
                var data = objeto.recvTime.substring(0,10);
                var dataRetorno = objeto.recvTime.substring(11, 19);
                return data + " " + dataRetorno;
                
            });

            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: listaDeDatas,
                    datasets: [{
                        label: 'Vazão',
                        data: listaDeValores,
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        },
        error: function () {
            alert("Erro ao carregar o conteúdo.");
        }
    });


   
});

