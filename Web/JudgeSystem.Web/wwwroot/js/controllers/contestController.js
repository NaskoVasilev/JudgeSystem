$('#course').change((e) => {
	let value = $(e.target).children("option:selected").val();
	let lessonType = $('#lessonType').children("option:selected").val();
	deleteAllFindLessons();
	getLessons(value, lessonType);
});

$('#lessonId').change((e) => {
	$($('#add-contest')[0]).removeAttr('disabled');
});

function getLessons(courseId, lessonType) {
    $.get(`/Administration/Contest/GetLessons?courseId=${courseId}`)
        .done(lessons => {
            let lessonsSelectList = $('#lessonId');
            lessonsSelectList.disabled = false;
            for (let lesson of lessons) {
                lessonsSelectList.append($(`<option value="${lesson.id}">${lesson.name}</option>`));
            }
        })
        .fail((error) => {
            console.log(error);
        });
}

function deleteAllFindLessons() {
	$('#lessonId > option:enabled').remove();
}

//Delete contest allowed ip address
let clickedDeleteButton;

$("#delete-confirm-btn").on('click', (e) => {

    if (clickedDeleteButton) {
        let ipAddressId = clickedDeleteButton.dataset.ipaddressid;
        let contestId = clickedDeleteButton.dataset.contestid;

        $.post('/Administration/Contest/RemoveAllowedIpAddress', { ipAddressId, contestId })
            .done((response) => {
                $(clickedDeleteButton.parentElement.parentElement).hide();
                showInfo(response);
            })
            .fail((error) => {
                showError(error.responseText);
            });
    }
});

$(".contest-allowed-ip-address-delete-btn").on('click', (e) => {
    clickedDeleteButton = $(e.target)[0];
});
