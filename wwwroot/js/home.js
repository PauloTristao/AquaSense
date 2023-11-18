

$(document).ready(function init() {
    const ctx = document.getElementById('myChart');
    url = "Api/RequestHistory";
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {
            debugger;
            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: data.recvTime,
                    datasets: [{
                        label: '# of Votes',
                        data: data.attrValue,
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

