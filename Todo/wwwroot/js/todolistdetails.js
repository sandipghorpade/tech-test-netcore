$(document).ready(function () {

    $("#toggleCompletedItems").click(function () {
        var link = $(this).find("strong");
        var isHideCompleted = link.text() === "Hide Completed Items";

        $(".list-group-item").each(function () {
            var itemTitle = $(this).find('input[name="item.IsDone"]');
            if (itemTitle.length>0) {
                $(this).toggle(!isHideCompleted);
            }
        });

        link.text(isHideCompleted ? "Show Completed Items" : "Hide Completed Items");
    });
});