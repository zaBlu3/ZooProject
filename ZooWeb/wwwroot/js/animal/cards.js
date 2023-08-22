const deleteAnimal = id => {
	$.ajax({
		contentType: "application/json",
		url: `https://localhost:7016/Animal/api/Delete/${id}`,
		type: "DELETE",
	}).done(() => location.reload())
}



$("#categoryDropdown").on("change", function () {
	var selectedCategory = this.value;

	var url = new URL(window.location.href);

	if (selectedCategory) {
		url.searchParams.set("selectedCategory", selectedCategory);
	} else {
		url.searchParams.delete("selectedCategory");
	}

	url.searchParams.delete("searchString");
	url.pathname = "/Animal/Cards/1"
	window.location.href = url;
});
$("#searchForm").on("submit", function (e) {
	e.preventDefault()
	var url = new URL(window.location.href);
	var searchString = $(this).find(":input[type='text']").val()
	url.searchParams.set("searchString", searchString);
	window.location.href = url;
});

$("a[data-sort-expression]").on("click", function () {
	var currentUrl = new URL(window.location.href);
	var sortExpression = $(this).data("sort-expression");
	currentUrl.searchParams.set("sortExpression", sortExpression);
	window.location.href = currentUrl;



});
$(".page-link").each(function () {
	var searchQuery = new URL(window.location.href).search;
	var currentHref = $(this).attr("href");
	$(this).attr("href", currentHref + searchQuery);

});

$(".card-body").hover(
	function () {
		$(this).parent().find(".card-img-top").addClass("hover-effect")
	},
	function () {
		$(this).parent().find(".card-img-top").removeClass("hover-effect")

	}
);