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
    updateTotalOdds()
    $('.odds').click(function () {
        // Se existir, remova windowContent-0
        $('#betCellId-0').remove();

        var oddsValue = $(this).find('.valueOdds').text();
        var oddsType = $(this).find('.typeOdds').text();
        var teamPoints1 = $(this).data('live-teampoints1');
        var teamPoints2 = $(this).data('live-teampoints2');
        var liveId = $(this).data('live-id');
        var livehome = $(this).data('live-home');
        var livevisiting = $(this).data('live-visiting');
        var betAmount = $('.betAmount').val();
        var homeTeamOdds = getOdds('OddsHome');
        var drawOdds = getOdds('OddsDraw');
        var visitingTeamOdds = getOdds('OddsVisiting');

        // Crie um novo ID para a betCell na WindowContent baseado no liveId
        var betCellId = 'betCell-' + liveId;
        // Verifique se a betCell já existe
        if ($('#' + betCellId).length === 0) {
            // Se não existir, crie uma nova dentro de betCell
            $('<div id="' + betCellId + '" class="text-white"></div>').appendTo('#betCell').css('height', '0').animate({ height: '105px', opacity: 1 }, "slow");
        }

        $.ajax({
            url: '/Bet/UpdateBetCell',
            type: 'POST',
            data: { ChosenOdds: oddsValue, SelectedOutcome: oddsType, HomeTeam: livehome, VisitingTeam: livevisiting, TeamPoints1: teamPoints1, TeamPoints2: teamPoints2 },
            success: function (partialViewHtml) {
                $('#' + betCellId).html(partialViewHtml);
                updateTotalOdds();
            }
        });

        $.ajax({
            url: '/Bet/UpdateBetResult',
            type: 'POST',
            data: { ChosenOdds: oddsValue, SelectedOutcome: oddsType, HomeTeam: livehome, VisitingTeam: livevisiting, HomeTeamOdds: homeTeamOdds, DrawOdds: drawOdds, VisitingTeamOdds: visitingTeamOdds, BetAmount: betAmount },
            success: function (partialViewHtml) {
                $('#betResult').html(partialViewHtml);
                updateTotalOdds();
            }
        });

        // Se a janela não estiver visível, exibe-a com uma animação de deslizamento
        if (!$("#windowContent").is(':visible')) {
            $("#windowContent").slideToggle(300);
        }
    });

    function getOdds(id) {
        var element = document.getElementById(id);
        return element && element.getElementsByClassName('valueOdds')[0] ? element.getElementsByClassName('valueOdds')[0].innerText : 0;
    }

    function updateTotalOdds() {
        var totalOdds = 0;
        $('.ChosenOdds').each(function () {
            var oddsValue = $(this).text().replace(',', '.');
            var numberValue = Number(oddsValue);
            if (!isNaN(numberValue)) {
                totalOdds += numberValue;
            }
        });
        $('#OddTotal').text(totalOdds.toFixed(2).replace('.', ','));

        if (totalOdds === 0) {
            localStorage.removeItem('totalAmountInvested');
        }
    }
   
    $('#windowHeader').click(function () {
        // Adicione windowContent-0 se ele não existir
        if ($('#betCellId-0').length === 0) {
            $('#betCell').append('<div id="betCellId-0" class="text-white" style="margin:1rem">Faça as suas apostas!</div>');
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


