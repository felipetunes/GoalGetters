
var heightRange = document.getElementById('heightRange');
var heightValue = document.getElementById('heightValue');

//// Se o valor inicial de heightRange for nulo, defina-o como 1.70
//if (heightRange.value === "") {
//    heightRange.value = "1.70";
//}

//// Atualize heightValue para refletir o valor atual de heightRange
//heightValue.innerText = heightRange.value;

function updateHeightValue(val) {
    heightValue.innerText = val;
}

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
