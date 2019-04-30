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

function fill_in_table()
{

}