function moveToNextField(currentField, fieldId) {
    if (currentField.value.length === 1) {
        document.getElementById(fieldId).focus();
    }
}