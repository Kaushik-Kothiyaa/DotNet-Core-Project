
$(document).ready(function () {
    loadDataTable();
});


function Delete(url) {
    swal({
        title: "Are you sure you want to delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true,   
        confirmButtonText: 'Yes, Delete it!'
    }).then((willDelete) => {
        console.log(willDelete);
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });

}

function loadDataTable() {
    dataTable = $('#tableData').DataTable({
        "ajax": {
            url: '/admin/product/getall'
        },
        "columns": [
            { data: 'title', width: '20%' },
            { data: 'author', width: '20%' },
            { data: 'isbn', width: '20%' },
            { data: 'listPrice', width: '20%' },
            { data: 'category.name', width: '10%' },   
            {
                data: 'id',
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2">
                            <i class="bi bi-pencil-fill"></i>Edit
                        </a>
                        <a onclick=Delete("/admin/product/delete/${data}") class="btn btn-danger mx-2">
                            <i class="bi bi-trash-fill"></i>Delete
                        </a>
                    </div>`;
                    width: '10%'
                }
            }   
        ]
    });
}

