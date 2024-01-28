
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