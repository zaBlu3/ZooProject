function deleteComment(id) {
    $.ajax({
        contentType: "application/json",
        url: `http://localhost:5099/api/comment/${id}`,
        type: "DELETE",
    })
        .done(() => {
            toastr.success('Comment Deleted successfully!', 'Success', {
                timeOut: 3000
            })
        }
        )
        .fail(
            (e) => toastr.error(e, 'Error', { timeOut: 3000 })
        )       
}
        
        
              
   

