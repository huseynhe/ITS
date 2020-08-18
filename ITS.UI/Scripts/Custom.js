var allres = "";

var ri = 0;

var isMain = 'False';


$(document).ajaxStart(function () {
    //$(".loader").css("display", "block");
});

$(document).ajaxStop(function () {
    //$(".loader").css("display", "none");
    $('[data-toggle="tooltip"]').tooltip();
});

var pov;
var eov;

function Search(elem, controller, action, param) {
    var nvalue = $(elem).val();

    if (pov == param && '' + eov + '' == '' + nvalue + '') {
        //alert('bbb');
        return false;
    }
    pov = param;
    eov = nvalue;

    if (ri == 0 || ri == 2) {

        var value = $(elem).val();
        ri = 1;
        var v = "";

        if (value == "") {
            v = "*";
        }

        if (param == 'LANGUAGE_CODE') {
            if (value != '' && value != null) {
                $('#divLANGUAGE_LEVEL').show();
            } else {
                $('#KNOWLEDGE_LEVEL_CODE').val('');
                $('#divLANGUAGE_LEVEL').hide();
            }
        }

        $.ajax({
            url: '/' + controller + '/' + action + '?' + param + '=' + $(elem).val() + v + '&prm=' + param + '&vl=' + $(elem).val() + v,
            type: 'GET',
            success: function (result) {

                $('#AjaxPaginationList').html(result);

                //$('#search-results-area').slideToggle("slide");
                $('#search-results-area').show();

                ri = 2;

                if ($(elem).val() != value) {

                    if (param == 'EndDate') {
                        param = 'BeginDate';

                        elem = $('#beginDate');
                    }
                    Search(elem, controller, action, param);
                }
            },
            error: function () {

            }
        });
    }
}
function Search2(elem, controller, action, param, datVal) {
    var nvalue = $(elem).val();

    if (pov == param && '' + eov + '' == '' + nvalue + '') {
        //alert('bbb');
        return false;
    }
    pov = param;
    eov = nvalue;

    if (ri == 0 || ri == 2) {

        var value = $(elem).val();
        ri = 1;
        var v = "";

        if (value == "") {
            v = "*";
        }

        if (param == 'LANGUAGE_CODE') {
            if (value != '' && value != null) {
                $('#divLANGUAGE_LEVEL').show();
            } else {
                $('#KNOWLEDGE_LEVEL_CODE').val('');
                $('#divLANGUAGE_LEVEL').hide();
            }
        }

        $.ajax({
            url: '/' + controller + '/' + action + '?' + 'userId' + '=' + datVal + '&prm=' + param + '&vl=' + $(elem).val() + v,
            type: 'GET',
            success: function (result) {

                $('#AjaxPaginationList').html(result);

                //$('#search-results-area').slideToggle("slide");
                $('#search-results-area').show();

                ri = 2;

                if ($(elem).val() != value) {

                    if (param == 'EndDate') {
                        param = 'BeginDate';

                        elem = $('#beginDate');
                    }
                    Search2(elem, controller, action, param);
                }
            },
            error: function () {

            }
        });
    }
}

function GetPlant(elem) {
    pId = $(elem).val();
    $(elem).parent().nextAll().remove();

    $.ajax({
        url: '/Person/RegionList?parentID=' + pId,
        type: 'GET',
        success: function (result) {
            $('#RegionID').val(pId);
            $(elem).parent().parent().append(result);
        },
        error: function () {

        }
    });
}

function GetSubject(elem) {
    pId = $(elem).val();
    $(elem).parent().nextAll().remove();

    $.ajax({
        url: '/Application/SubjectList?parentID=' + pId,
        type: 'GET',
        success: function (result) {
            $('#SubjectType').val(pId);
            $(elem).parent().parent().append(result);
        },
        error: function () {

        }
    });
}

var allowfiletype = ["image/jpeg", "image/png", "application/pdf", "application/octet-stream", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"];
var ftypes = ".pdf, .jpeg, .jpg, .png, .edoc, .doc, .docx";
var tfilefieldtemplate;
var ffilefieldtemplate;
var filefieldtemplate;

function chosefiles(elem) {
    //var requiredfs = 2;
    var requiredfs = (Math.round((($('#uploadFileSize').val() / 1024) / 1024) * 100) / 100);

    var totalfs = 0;
    var fiv = 0;
    var flength = elem.files.length;
    var froot = $(elem).parent();
    var filenames = "";

    //alert(requiredfs);
    $('#btnUploadFile').removeClass('disabled');

    if (froot.find('.scope').length == 0) {
        if (froot.find('.true').length == 0)
            tfilefieldtemplate = froot.html();
        if (froot.find('.false').length == 0)
            ffilefieldtemplate = froot.html();
    }

    if (froot.find('.true').length == 0)
        filefieldtemplate = tfilefieldtemplate;
    if (froot.find('.false').length == 0)
        filefieldtemplate = ffilefieldtemplate;

    for (l = 0; l < flength; l++) {

        if ($.inArray(elem.files[l].type, allowfiletype) < 0) {
            var ext = elem.files[l].name.split('.').pop().toLowerCase();
            if (ext != 'edoc') {
                fiv = 1;
            }
        }


        totalfs = totalfs + parseInt(elem.files[l].size, 10) / 1024;

        filenames = filenames + elem.files[l].name + ' - ' + (Math.round(((parseInt(elem.files[l].size, 10) / 1024)) / 1024 * 100) / 100) + ' mb' + '\n';
    }

    totalfs = (Math.round((totalfs / 1024) * 100) / 100);

    froot.find('.sel').html('<span class="scope" style="font-size: 16px;font-weight: bold;">' + flength + '</span> şəkil seçilib, həcmi <span style="font-size: 16px;font-weight: bold;">' + totalfs + '</span> mb <span title="' + filenames + '" style="cursor:pointer;color:#428bca;" class="glyphicon glyphicon-info-sign"></span>');

    $('span').tooltip();

    if (totalfs > requiredfs) {
        $('#btnUploadFile').addClass('disabled');
        alert('Seçilmiş şəkillərin həcmi ' + requiredfs + ' MB-dan az olmalıdır. \n\n Sizin sənədin həcmi ' + totalfs + ' MB');
        froot.html(filefieldtemplate);
    }

    //if (fiv == 1) {
    //    $('#btnUploadFile').addClass('disabled');
    //    alert('Seçilmiş şəkil tipinə icazə verilmir. \n\nQəbul olunan şəkil tipləri: ' + ftypes);
    //    froot.html(filefieldtemplate);
    //}

}


function sendFiles() {

    var formData = new FormData();
    var len = $('#fup')[0].files.length;

    for (i = 0; i < len; i++) {
        formData.append('file', $('#fup')[0].files[i]);
    }

    //formData.append('documentType', documentType)
    if (i == 0) {
        alert('fayl seçilməyib');
        return false;
    }

    
    var applicationId = $('#applicationId').val();
    formData.append('applicationId', applicationId);

    console.log(formData);

    $.ajax({
        url: '/Application/File',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            GetAttachFileTemp(applicationId);
            GetAttachFileList(applicationId);
        },
        error: function () {
            alert('səhv baş verdi');
        }
    });
}


function GetAttachFileTemp(applicationId) {

    $('.attachFileField').html('');
    $('#applicationId').val(applicationId);

    $.ajax({
        url: '/Application/FileTemplate',
        type: 'GET',
        success: function (result) {
            //$(attObj).parent().parent().parent().parent().find('.attachFileField').show(500);
            //$(attObj).parent().parent().parent().find('.attachFileField').html(result);

            $('.attachFileField').html(result);
            $('.attachFileField').show(500);

            GetAttachFileList(applicationId);

        },
        error: function () {

        }
    });
};


function GetAttachFileList(applicationId) {
    //if (currentId != ocId) {
    //    ocId = currentId;
    //    $('.attachFileList').hide(500);
    //}
    $('.attachFileList').html('');
    $('#applicationId').val(applicationId);


    $.ajax({
        url: '/Application/FileList?applicationId=' + applicationId,
        type: 'GET',
        success: function (result) {
            $('.attachFileList').html(result);
            $('.attachFileList').show(500);
            //$(attObj).parent().parent().find('.attachFileList').html(result);
            //$(attObj).parent().parent().find('.attachFileList').show(500);
        },
        error: function () {

        }
    });
};

function DeleteApplicationDocumentById(id) {

    var applicationId = $('#applicationId').val();

    $.ajax({
        url: '/Application/DeleteApplicationDocumentById?id=' + id,
        type: 'GET',
        success: function (result) {
            GetAttachFileTemp(applicationId);
            GetAttachFileList(applicationId);
        },
        error: function () {

        }
    });
}


function choseProfilFile(elem) {
    //var requiredfs = 2;
    var requiredfs = (Math.round((($('#uploadFileSize').val() / 1024) / 1024) * 100) / 100);

    var totalfs = 0;
    var fiv = 0;
    var flength = elem.files.length;
    var froot = $(elem).parent();
    var filenames = "";

    //alert(requiredfs);
    $('#btnUploadFile').removeClass('disabled');

    if (froot.find('.scope').length == 0) {
        if (froot.find('.true').length == 0)
            tfilefieldtemplate = froot.html();
        if (froot.find('.false').length == 0)
            ffilefieldtemplate = froot.html();
    }

    if (froot.find('.true').length == 0)
        filefieldtemplate = tfilefieldtemplate;
    if (froot.find('.false').length == 0)
        filefieldtemplate = ffilefieldtemplate;

    for (l = 0; l < flength; l++) {

        if ($.inArray(elem.files[l].type, allowfiletype) < 0) {
            var ext = elem.files[l].name.split('.').pop().toLowerCase();
            if (ext != 'edoc') {
                fiv = 1;
            }
        }


        totalfs = totalfs + parseInt(elem.files[l].size, 10) / 1024;

        filenames = filenames + elem.files[l].name + ' - ' + (Math.round(((parseInt(elem.files[l].size, 10) / 1024)) / 1024 * 100) / 100) + ' mb' + '\n';
    }

    totalfs = (Math.round((totalfs / 1024) * 100) / 100);

    froot.find('.sel').html('<span class="scope" style="font-size: 16px;font-weight: bold;">' + flength + '</span> şəkil seçilib, həcmi <span style="font-size: 16px;font-weight: bold;">' + totalfs + '</span> mb <span title="' + filenames + '" style="cursor:pointer;color:#428bca;" class="glyphicon glyphicon-info-sign"></span>');

    $('span').tooltip();

    if (totalfs > requiredfs) {
        $('#btnUploadFile').addClass('disabled');
        alert('Seçilmiş şəkillərin həcmi ' + requiredfs + ' MB-dan az olmalıdır. \n\n Sizin sənədin həcmi ' + totalfs + ' MB');
        froot.html(filefieldtemplate);
    }

    //if (fiv == 1) {
    //    $('#btnUploadFile').addClass('disabled');
    //    alert('Seçilmiş şəkil tipinə icazə verilmir. \n\nQəbul olunan şəkil tipləri: ' + ftypes);
    //    froot.html(filefieldtemplate);
    //}

}


function sendProfileFiles() {

    var formData = new FormData();
    var len = $('#fup')[0].files.length;

    for (i = 0; i < len; i++) {
        formData.append('file', $('#fup')[0].files[i]);
    }

    //formData.append('documentType', documentType)
    if (i == 0) {
        alert('fayl seçilməyib');
        return false;
    }

    var personID = $('#ID').val();
    formData.append('personID', personID);

    $.ajax({
        url: '/Person/File',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            GetProfilFileTemp(personID);
            GetPersonFileList(personID);
        },
        error: function () {
            alert('səhv baş verdi');
        }
    });
}


function GetProfilFileTemp(personID) {

    $('.attachFileField').html('');
    $('#ID').val(personID);

    $.ajax({
        url: '/Person/FileTemplate',
        type: 'GET',
        success: function (result) {
            //$(attObj).parent().parent().parent().parent().find('.attachFileField').show(500);
            //$(attObj).parent().parent().parent().find('.attachFileField').html(result);

            $('.attachFileField').html(result);
            $('.attachFileField').show(500);

            GetPersonFileList(personID);

        },
        error: function () {

        }
    });
};


function GetPersonFileList(personID) {
    //if (currentId != ocId) {
    //    ocId = currentId;
    //    $('.attachFileList').hide(500);
    //}
    $('.attachFileList').html('');
    $('#ID').val(personID);


    $.ajax({
        url: '/Person/FileList?personID=' + personID,
        type: 'GET',
        success: function (result) {
            $('.attachFileList').html(result);
            $('.attachFileList').show(500);
            //$(attObj).parent().parent().find('.attachFileList').html(result);
            //$(attObj).parent().parent().find('.attachFileList').show(500);
        },
        error: function () {

        }
    });
};

function DeleteProfilFileById(personID) {

    //var personID = $('#ID').val();

    $.ajax({
        url: '/Person/DeleteProfilFileById?personID=' + personID,
        type: 'GET',
        success: function (result) {
            GetProfilFileTemp(personID);
            GetPersonFileList(personID);
        },
        error: function () {

        }
    });
}

function sendOutFiles() {

    var formData = new FormData();
    var len = $('#fup')[0].files.length;

    for (i = 0; i < len; i++) {
        formData.append('file', $('#fup')[0].files[i]);
    }

    //formData.append('documentType', documentType)
    if (i == 0) {
        alert('fayl seçilməyib');
        return false;
    }

    var applicationId = $('#applicationId').val();
    formData.append('applicationId', applicationId);

    $.ajax({
        url: '/OutApplication/File',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            GetOutAttachFileTemp(applicationId);
            GetOutAttachFileList(applicationId);
        },
        error: function () {
            alert('səhv baş verdi');
        }
    });
}

function GetOutAttachFileTemp(applicationId) {

    $('.attachFileField').html('');
    $('#applicationId').val(applicationId);

    $.ajax({
        url: '/OutApplication/FileTemplate',
        type: 'GET',
        success: function (result) {
            //$(attObj).parent().parent().parent().parent().find('.attachFileField').show(500);
            //$(attObj).parent().parent().parent().find('.attachFileField').html(result);

            $('.attachFileField').html(result);
            $('.attachFileField').show(500);

            GetOutAttachFileList(applicationId);

        },
        error: function () {

        }
    });
};


function GetOutAttachFileList(applicationId) {
    //if (currentId != ocId) {
    //    ocId = currentId;
    //    $('.attachFileList').hide(500);
    //}
    $('.attachFileList').html('');
    $('#applicationId').val(applicationId);


    $.ajax({
        url: '/OutApplication/FileList?applicationId=' + applicationId,
        type: 'GET',
        success: function (result) {
            $('.attachFileList').html(result);
            $('.attachFileList').show(500);
            //$(attObj).parent().parent().find('.attachFileList').html(result);
            //$(attObj).parent().parent().find('.attachFileList').show(500);
        },
        error: function () {

        }
    });
};

function DeleteOutApplicationDocumentById(id) {

    var applicationId = $('#applicationId').val();

    $.ajax({
        url: '/OutApplication/DeleteOutApplicationDocumentById?id=' + id,
        type: 'GET',
        success: function (result) {
            GetOutAttachFileTemp(applicationId);
            GetOutAttachFileList(applicationId);
        },
        error: function () {

        }
    });
}


function choseAdmfiles(elem) {
    //var requiredfs = 2;
    var requiredfs = (Math.round((($('#uploadFileSize').val() / 1024) / 1024) * 100) / 100);

    var totalfs = 0;
    var fiv = 0;
    var flength = elem.files.length;
    var froot = $(elem).parent();
    var filenames = "";

    //alert(requiredfs);
    $('#btnUploadFile').removeClass('disabled');

    if (froot.find('.scope').length == 0) {
        if (froot.find('.true').length == 0)
            tfilefieldtemplate = froot.html();
        if (froot.find('.false').length == 0)
            ffilefieldtemplate = froot.html();
    }

    if (froot.find('.true').length == 0)
        filefieldtemplate = tfilefieldtemplate;
    if (froot.find('.false').length == 0)
        filefieldtemplate = ffilefieldtemplate;

    for (l = 0; l < flength; l++) {

        if ($.inArray(elem.files[l].type, allowfiletype) < 0) {
            var ext = elem.files[l].name.split('.').pop().toLowerCase();
            if (ext != 'edoc') {
                fiv = 1;
            }
        }


        totalfs = totalfs + parseInt(elem.files[l].size, 10) / 1024;

        filenames = filenames + elem.files[l].name + ' - ' + (Math.round(((parseInt(elem.files[l].size, 10) / 1024)) / 1024 * 100) / 100) + ' mb' + '\n';
    }

    totalfs = (Math.round((totalfs / 1024) * 100) / 100);

    froot.find('.sel').html('<span class="scope" style="font-size: 16px;font-weight: bold;">' + flength + '</span> şəkil seçilib, həcmi <span style="font-size: 16px;font-weight: bold;">' + totalfs + '</span> mb <span title="' + filenames + '" style="cursor:pointer;color:#428bca;" class="glyphicon glyphicon-info-sign"></span>');

    $('span').tooltip();

    if (totalfs > requiredfs) {
        $('#btnUploadFile').addClass('disabled');
        alert('Seçilmiş şəkillərin həcmi ' + requiredfs + ' MB-dan az olmalıdır. \n\n Sizin sənədin həcmi ' + totalfs + ' MB');
        froot.html(filefieldtemplate);
    }

    //if (fiv == 1) {
    //    $('#btnUploadFile').addClass('disabled');
    //    alert('Seçilmiş şəkil tipinə icazə verilmir. \n\nQəbul olunan şəkil tipləri: ' + ftypes);
    //    froot.html(filefieldtemplate);
    //}

}


function sendAdmFiles() {

    var formData = new FormData();
    var len = $('#fup')[0].files.length;

    for (i = 0; i < len; i++) {
        formData.append('file', $('#fup')[0].files[i]);
    }

    //formData.append('documentType', documentType)
    if (i == 0) {
        alert('fayl seçilməyib');
        return false;
    }


    var applicationId = $('#applicationId').val();
    formData.append('applicationId', applicationId);

    console.log(formData);

    $.ajax({
        url: '/AdministrativeApplication/File',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            GetAdmAttachFileTemp(applicationId);
            GetAdmAttachFileList(applicationId);
        },
        error: function () {
            alert('səhv baş verdi');
        }
    });
}


function GetAdmAttachFileTemp(applicationId) {

    $('.attachFileField').html('');
    $('#applicationId').val(applicationId);

    $.ajax({
        url: '/AdministrativeApplication/FileTemplate',
        type: 'GET',
        success: function (result) {
            //$(attObj).parent().parent().parent().parent().find('.attachFileField').show(500);
            //$(attObj).parent().parent().parent().find('.attachFileField').html(result);

            $('.attachFileField').html(result);
            $('.attachFileField').show(500);

            GetAdmAttachFileList(applicationId);

        },
        error: function () {

        }
    });
};


function GetAdmAttachFileList(applicationId) {
    //if (currentId != ocId) {
    //    ocId = currentId;
    //    $('.attachFileList').hide(500);
    //}
    $('.attachFileList').html('');
    $('#applicationId').val(applicationId);


    $.ajax({
        url: '/AdministrativeApplication/FileList?applicationId=' + applicationId,
        type: 'GET',
        success: function (result) {
            $('.attachFileList').html(result);
            $('.attachFileList').show(500);
            //$(attObj).parent().parent().find('.attachFileList').html(result);
            //$(attObj).parent().parent().find('.attachFileList').show(500);
        },
        error: function () {

        }
    });
};

function DeleteAdmDocumentById(id) {

    var applicationId = $('#applicationId').val();

    $.ajax({
        url: '/AdministrativeApplication/DeleteDocumentById?id=' + id,
        type: 'GET',
        success: function (result) {
            GetAdmAttachFileTemp(applicationId);
            GetAdmAttachFileList(applicationId);
        },
        error: function () {

        }
    });
}

function GetPerson(elem, SId) {
    $.ajax({
        url: '/WorkStructure/Person?SId=' + SId,
        type: 'GET',
        success: function (result) {
            $(elem).parent().find('.person').html(result);
        },
        error: function () {

        }
    });
};


function Searchcontactinfo(elem) {

    if ($(elem).val().length < 2) {
        $('#personlist').html('');
        return false;
    }

    $('#personlist').show();
    $('#personlist').html('<div class="text-center"><img src="/Images/loading.gif" /></div>');

    if (ri == 0 || ri == 2) {

        var value = $(elem).val();
        ri = 1;

        $.ajax({
            url: '/WorkStructure/Person?prm=Name&vl=' + $(elem).val(),
            type: 'GET',
            success: function (result) {

                $('#personlist').html(result);

                $('#personlist').show();

                ri = 2;
                if ($(elem).val() != value) {
                    Searchcontactinfo(elem);
                }

            },
            error: function () {

            }
        });
    }
}


