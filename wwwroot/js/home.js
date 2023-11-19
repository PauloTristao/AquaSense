var chart;

$(document).ready(function () {
    init();
    setInterval(atualizar, 2000);
})

function init() {
    //$('#myChart').remove(); // this is my <canvas> element
    //$('#myContainer').append('<canvas id="myChart"><canvas>');
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
                var data = objeto.recvTime.substring(0, 10);
                var dataRetorno = objeto.recvTime.substring(11, 19);
                return data + " " + dataRetorno;

            });

            chart = new Chart(ctx, {
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

            chart.update();
        },
        error: function () {
            alert("Erro ao carregar o conteúdo.");
        }

    });

}

function atualizar() {
    url = "Api/RequestHistory";
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {

            var listaDeValores = data.map(function (objeto) {
                return objeto.attrValue;
            });

            var listaDeDatas = data.map(function (objeto) {
                var data = objeto.recvTime.substring(0, 10);
                var dataRetorno = objeto.recvTime.substring(11, 19);
                return data + " " + dataRetorno;

            });

            chart.data.datasets[0].data = listaDeValores;
            chart.data.labels = listaDeDatas;

            chart.update();
        },
        error: function () {
            alert("Erro ao carregar o conteúdo.");
        }

    });
}



