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
        // Se existir, remova windowContent-0
        $('#windowContent-0').remove();

        var oddsValue = $(this).find('.valueOdds').text();
        var oddsType = $(this).find('.typeOdds').text();
        var liveId = $(this).data('live-id');
        var livehome = $(this).data('live-home');
        var livevisiting = $(this).data('live-visiting');
        var homeTeamOdds = document.getElementById('OddsHome').getElementsByClassName('valueOdds')[0].innerText;
        var drawOdds = document.getElementById('OddsDraw').getElementsByClassName('valueOdds')[0].innerText;
        var visitingTeamOdds = document.getElementById('OddsVisiting').getElementsByClassName('valueOdds')[0].innerText;
        var betAmount = $('#betAmount').val();

        // Crie um novo ID para a windowContent baseado no liveId
        var windowContentId = 'windowContent-' + liveId;

        // Verifique se a windowContent já existe
        if ($('#' + windowContentId).length === 0) {
            // Se não existir, crie uma nova dentro de windowContent
            // Use a função hide() para inicialmente esconder o novo windowContent
            $('<div id="' + windowContentId + '" class="text-white"></div>').appendTo('#windowContent').css('height', '0').animate({ height: '122px', opacity: 1 }, "slow");
        }

        $.ajax({
            url: '/Bet/UpdatePartialView',
            type: 'POST',
            data: { ChosenOdds: oddsValue, SelectedOutcome: oddsType, MatchId: liveId, HomeTeam: livehome, VisitingTeam: livevisiting, HomeTeamOdds: homeTeamOdds, DrawOdds: drawOdds, VisitingTeamOdds: visitingTeamOdds, BetAmount: betAmount },
            success: function (partialViewHtml) {
                $('#' + windowContentId).html(partialViewHtml);
            }
        });

        // Se a janela não estiver visível, exibe-a com uma animação de deslizamento
        if (!$("#windowContent").is(':visible')) {
            $("#windowContent").slideToggle(300);
        }
    });

    $('#windowHeader').click(function () {
        // Adicione windowContent-0 se ele não existir
        if ($('#windowContent-0').length === 0) {
            $('#windowContent').append('<div id="windowContent-0" class="text-white" style="margin:1rem">Faça as suas apostas!</div>');
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

