$(document).ready(function () {
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

// Se o valor inicial de heightRange for nulo, defina-o como 1.70
if (heightRange.value === "") {
    heightRange.value = "1.70";
}

// Atualize heightValue para refletir o valor atual de heightRange
heightValue.innerText = heightRange.value;

function updateHeightValue(val) {
    heightValue.innerText = val;
}