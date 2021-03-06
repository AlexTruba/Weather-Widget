﻿$(document).ready(function () {
    $(".week .day-js").first().addClass("hide");
    $(".all-days .daily-weather").first().addClass("active");

    if (document.getElementById("chart") !== null) DrawDiagram(0);
    
    $(".mobile__bar").click(function () {
        $(this).toggleClass("active");
        $(".header__menu").slideToggle(300);
    });
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
                if ($(".log-table tr").length === 2) {
                    $(".log-table").parent().remove();
                    $("h2.change").text("Історія запитів пуста!");
                }
                tr.remove();
            }
        });
    });
    $(".elect__table input").not(".new-city").focusout(function () {
        let name = $(this);
        console.log(name.parents("tr").attr("data-id"));
        console.log(name.val());
        if (name.val().length!==0) {
            $.ajax({
                type: "POST",
                url: "/ElectCity/EditElectTown",
                data: { id: name.parents("tr").attr("data-id"), name: name.val() },
                dataType: "json",
                success: function (data) {
                }
            });
        }
    });
    $(".elect__table .remove-city-js").click(function () {
        let tr = $(this).parents("tr");
        $.ajax({
            type: "POST",
            url: "/ElectCity/RemoveElectTown",
            data: { id: tr.attr("data-id")},
            dataType: "json",
            success: function (data) {
                tr.remove();
            }
        });
    });

    $(".elect__table .add-city-js").click(function () {
        var name = $(".elect__table .new-city");
        if (name.val().length !== 0) {
            $.ajax({
                type: "POST",
                url: "/ElectCity/AddElectTown",
                data: { name: name.val() },
                dataType: "json",
                success: function (data) {
                    if (data.status === 200) {
                        console.log(data);
                        $(".elect__table tbody tr").last().before(`<tr data-id="` + data.id +
                            `">
                            <td><input type="text" name="name" value="`+ name.val() +
                            `"/></td>
                            <td>
                                <a class="add-log-js plus">
                                    <div></div>
                                    <div></div>
                                </a>
                            </td>
                        </tr>`);
                        name.val("");
                    }
                }
            });
        }
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
