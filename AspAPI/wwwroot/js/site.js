function openCourseModal(parameters) {
    var headerTxt = parameters.txt;
    var id = parameters.data;
    var url = parameters.url;
    var modal = $('#modal');

    if (id === undefined || url === undefined) {
        alert('Data miss')
    }

    $.ajax({
        type: 'GET',
        url: url,
        data: { "id": id },
        success: function (response) {
            $('.modal-dialog');
            modal.find(".modal-header").find('.modal-title').html(headerTxt);
            modal.find(".modal-body").html(response);
            modal.modal('show')
        },
        failure: function () {
            modal.modal('hide')
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
};

function openLoginModal(parameters) {
    var headerTxt = parameters.txt;
    var url = parameters.url;
    var modal = $('#modal');

    if (url === undefined) {
        alert('Data miss')
    }

    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            $('.modal-dialog');
            modal.find(".modal-header").find('.modal-title').html(headerTxt);
            modal.find(".modal-body").html(response);
            modal.modal('show')
        },
        failure: function () {
            modal.modal('hide')
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
};