function initializeDateTimePicker(id) {
    $(`#${id}`).datetimepicker({
        format: 'd/m/Y H:i',
        lang: 'en',
    });
}