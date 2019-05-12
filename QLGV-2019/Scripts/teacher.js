function fill_in_form()
{
    var id = $('#Id').val();
    if(id != null)
    {
           $.ajax({
               url: '/Member/AllInfo/?Id=' + id,
                type: 'GET',
                contentType: "application/json;charset=UTF-8",
                dataType: "json",
                success: function (data) {
                    $('#First_Name').val(data.name);
                    $('#Last_Name').val(data.sub_name);
                }
            });
    }
}

function fill_in_mark()
{
    var id = $('#Id').val();
    if (id != null) {
        $.ajax({
            url: '/Member/GetAllSubject/?Id=' + id,
            type: 'GET',
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (data) {
                $('#Subject_Name').val(data.sub_name);
            }
        });
    }
}

function btn_block(id)
{
    $(id).hide();
}

function pass_confirm()
{
    var a = $('#a').val();
    var b = $('#b').val();
    if (a != b) {
        $('#valid-password').text('password no match');
        $('#capnhat').attr('disabled', true);
    }
    else {
        $('#valid-password').attr('hidden', true);
        $('#capnhat').attr('disabled', false);
    }
}

function fill_select()
{
    var id = $('#School_Year').val();
    if(id != null)
    {
        $.ajax({
            url: '/Subject/ListYearTerm/?Id=' + id,
            type: 'GET',
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (data) {
                $.each(data, function (key, value) {
                    $('#Year_Term').append('<option value=' + value.year_term + '>' + value.year_term + '</option>');
                })

            }
        });
    }
}

function check_duclicte_user()
{
    var id = $('Id_number').val();
    if (id != null)
    {
        $.ajax({
            url: '/Member/GetUser/?Id=' + id,
            type: 'GET',
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (data) {
                $('#error').text('THIS ACCOUNT IS EXISTED');
                $('#error').show();
                $('#create_user').attr('disabled', true);
            },
            error: function () {
                $('#error').hide();
                $('#create_user').attr('disabled', false);
            }
        });
    }
   
}