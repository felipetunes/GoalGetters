document.addEventListener('DOMContentLoaded', function () {
    var $homeTeamSelect = $('#homeTeamSelect');
    var $visitingTeamSelect = $('#visitingTeamSelect');

    function updateTeams(idChampionship) {
        $.get('http://localhost:8080/api/v1/team/getbychampionship/' + idChampionship, function (data) {
            $homeTeamSelect.empty();
            $visitingTeamSelect.empty();

            data.forEach(function (team, index) {
                $homeTeamSelect.append('<option value="' + team.id + '">' + team.name + '</option>');
                $visitingTeamSelect.append('<option value="' + team.id + '">' + team.name + '</option>');
                if (index === 1) {
                    $visitingTeamSelect.val(team.id);
                }
            });

            $homeTeamSelect.change(function () {
                var selected = $(this).val();
                $visitingTeamSelect.find('option').prop('disabled', false);
                $visitingTeamSelect.find('option[value=' + selected + ']').prop('disabled', true);
            }).change(); // Dispara o evento change imediatamente

            $visitingTeamSelect.change(function () {
                var selected = $(this).val();
                $homeTeamSelect.find('option').prop('disabled', false);
                $homeTeamSelect.find('option[value=' + selected + ']').prop('disabled', true);
            }).change(); // Dispara o evento change imediatamente

        });
    }

    $('#IdChampionship').change(function () {
        var idChampionship = $(this).val(); // Obtenha o valor do campo IdChampionship
        updateTeams(idChampionship);
    }).change(); // Dispara o evento change imediatamente
});

$(document).ready(function () {
    $("#windowContent").hide();

    $('.odds').click(function () {
        var oddsValue = parseFloat($(this).find('.valueOdds').text().replace(',', '.'));
        var oddsType = $(this).find('.typeOdds').text();

        // Encontre o elemento pai que contém os elementos homeTeam e visitingTeam
        var parentElement = $(this).closest('container');

        // Obtenha os elementos homeTeam e visitingTeam dentro do elemento pai
        var homeTeamElement = parentElement.find('.homeTeam')[0];
        var visitingTeamElement = parentElement.find('.visitingTeam')[0];

        // Obtenha o conteúdo de texto dos elementos
        var liveHomeTeamName = homeTeamElement.textContent;
        var liveVisitingTeamName = visitingTeamElement.textContent;

        // Defina um valor padrão para amountInvested
        var defaultAmountInvested = 20.00;

        // Obtenha os elementos scoreboard dentro do elemento pai
        var scoreboardElements = parentElement.find('.scoreboard');

        // Obtenha o conteúdo de texto dos elementos
        var teamPoints1 = $(scoreboardElements[0]).text();
        var teamPoints2 = $(scoreboardElements[1]).text();

    

        // Construa a string de forma condicional
        var teamString = '';
        if (oddsType == 1) {
            teamString = liveHomeTeamName;
        } else if (oddsType == 2) {
            teamString =  liveVisitingTeamName;
        } else {
            teamString = 'Empate';
        }

        // Atualiza o conteúdo da janela com as informações obtidas
        $("#windowContent").html(
            '<p>' + teamString + '</p>' +
            '<p><span>' + liveHomeTeamName + '</span>' + ' vs ' + '<span>' + liveVisitingTeamName + '</span></p>' +
            '<p>' + teamPoints1 + 'x' + teamPoints2 + '</p>' +
            '<p><strong><span id="oddsValue">' + oddsValue + '</span></strong></p>' + 
            '<div><input type="number" id="amountInvested" class="form-control" value="' + defaultAmountInvested.toFixed(2) + '" /></div>'
        );

        // Obtenha o valor do amountInvested e some com o oddsValue
        var total = oddsValue * defaultAmountInvested;

        // Formate o total para usar uma vírgula como separador decimal e duas casas decimais
        total = total.toFixed(2).toString().replace('.', ',');

        // Adicione o total à janela de conteúdo
        $("#windowContent").append('<p><strong>Ganhos Possíveis:</strong> R$ ' + total + '</p>');

        // Se a janela não estiver visível, exibe-a com uma animação de deslizamento
        if (!$("#windowContent").is(':visible')) {
            $("#windowContent").slideToggle(300);
        }
    });

    $(document).on('input', '#amountInvested', function () {
        var oddsValue = parseFloat($("#oddsValue").text().replace(',', '.'));
        var amountInvested = parseFloat($(this).val().replace(',', '.'));
        var total = oddsValue * amountInvested;
        total = total.toFixed(2).toString().replace('.', ',');
        $("#windowContent").find('p:contains("Total:")').html('<strong>Total:</strong> R$ ' + total);
    });


    $('#windowHeader').click(function () {
        var oddsValue = $(this).find('.valueOdds').text();
        if (oddsValue == '') {
            $("#windowContent").html('<p><strong>Faça suas apostas</strong></p>');
        }
        $("#windowContent").slideToggle(300);
    });

    // Buscar dados da API quando o modal é aberto
    $('#myModal').on('shown.bs.modal', function () {
        $.ajax({
            url: 'http://localhost:8080/api/v1/team/getall',  // Substitua pelo URL da sua API
            type: 'GET',
            success: function (data) {
                // Processar os dados retornados pela API
                var teams = data;  // Substitua pelo nome correto do campo de dados
                var teamList = $('#teamList');  // Substitua pelo ID do elemento onde você quer mostrar a lista de times
                teamList.empty();
                for (var i = 0; i < teams.length; i++) {
                    teamList.append('<option value="' + teams[i].id + '">' + teams[i].name + '</option>');  // Substitua 'id' e 'name' pelos campos corretos do objeto team
                }
            },
            error: function () {
                console.log('Erro ao buscar dados da API');
            }
        });
    });

    // Atualizar o campo 'IdTeam' com o ID do time selecionado e fechar o modal quando o botão 'saveChoice' for clicado
    $('#saveChoice').click(function () {
        var selectedTeamId = $('#teamList').val();
        $('#idTeamField').val(selectedTeamId);
        $('#myModal').modal('hide');
    });
});


var heightRange = document.getElementById('heightRange');
var heightValue = document.getElementById('heightValue');

if (heightRange && heightValue) {
    // Se o valor inicial de heightRange for nulo, defina-o como 1.70
    if (heightRange.value === "") {
        heightRange.value = "1.70";
    }

    // Atualize heightValue para refletir o valor atual de heightRange
    heightValue.innerText = heightRange.value;
}

function updateHeightValue(val) {
    if (heightValue) {
        heightValue.innerText = val;
    }
}

