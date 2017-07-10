$(document).ready(function () {
    $(".week .day-js").first().addClass("hide");
    $(".all-days .daily-weather").first().addClass("active");

    if (document.getElementById("chart") !== null) DrawDiagram(0);

    $(".week .day-js").click(function () {
        $(this).parent().find(".hide").removeClass("hide");
        $(this).addClass("hide");
        $(".all-days .active").removeClass("active");
        let index = $(".week .day-js").index($(".hide"));
        $(".all-days .daily-weather").eq(index).addClass("active");
        DrawDiagram(parseInt(index));
    });

    $(".remove-log-js").click(function () {
        let tr = $(this).parents("tr");

        console.log(tr.attr("data-id"));
        $.ajax({
            type: "POST",
            url: "/Home/RemoveOneLog",
            data: { id: parseInt(tr.attr("data-id")) },
            dataType: "json",
            success: function (data) {
                console.log($(".log-table tr").length);
                if ($(".log-table tr").length == 2) {
                    $(".log-table").parent().remove();
                    $("h2.change").text("Історія запитів пуста!");
                }
                tr.remove();
            }
        });
    });
    
});

function DrawDiagram(index) {
    $(".diagram iframe").remove();
    var ctx = document.getElementById("chart").getContext('2d');
    let array = [Number((data[index].temp.morn).toFixed(1)), Number((data[index].temp.eve).toFixed(1)), Number((data[index].temp.night).toFixed(1))];
    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: ["Вранці", "Вдень", "Вночі"],
            datasets: [{
                label: 'Температура протягом дня',
                fill: false,
                backgroundColor: "#fff",
                borderColor:"#fff" ,
                data: array,
                //backgroundColor: [
                //    'rgba(255, 99, 132, 0.2)',
                //    'rgba(54, 162, 235, 0.2)',
                //    'rgba(255, 206, 86, 0.2)',
                //],
                //borderColor: [
                //    'rgba(255,99,132,1)',
                //    'rgba(255, 255, 255, 1)',
                //    'rgba(255, 206, 86, 1)',
                //],
                borderWidth: 4
            }]
        },
        options: {
            legend: {
                labels: {
                    fontColor: 'white'
                }
            },
            scales: {
                yAxes: [{
                    ticks: {
                        fontSize: 16,
                        beginAtZero: false,
                        fontColor: 'white'
                    },
                }],
                xAxes: [{
                    ticks: {
                        fontSize: 16,
                        fontColor: 'white'
                    },
                }]
            } 
        }
    });
}
