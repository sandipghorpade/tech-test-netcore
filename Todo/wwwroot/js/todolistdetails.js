$(document).ready(function () {

    var isAscending = true;
    var $listItems = $(".list-group-item:not(:first):not(:nth-child(2))");
  
    $("#toggleCompletedItems").click(function () {
        var link = $(this).find("strong");
        var isHideCompleted = link.text() === "Hide Completed Items";

        $(".list-group-item").each(function () {
            var itemTitle = $(this).find('input[name="item.IsDone"]');
            if (itemTitle.length > 0) {
                $(this).toggle(!isHideCompleted);
            }
        });

        link.text(isHideCompleted ? "Show Completed Items" : "Hide Completed Items");
    });

    $('#createItemModal').on('shown.bs.modal', function (e) {
        var todoListId = $(this).data('todo-list-id');
        var url = getTodoItemFormPartialViewUrl + '?todoListId=' + todoListId;

        $.get(url, function (data) {
            $('#createItemModal .modal-body').html(data);
        });
    }); 

    $("#orderByRank").on('click', function (e) {

        $listItems.sort(function (a, b) {
            var rankA = parseInt($(a).find(".col-md-4:nth-child(2)").text());
            var rankB = parseInt($(b).find(".col-md-4:nth-child(2)").text());

            if (isNaN(rankA)) rankA = 0;
            if (isNaN(rankB)) rankB = 0;

            if (isAscending) {
                return rankA - rankB;
            } else {
                return rankB - rankA;
            }
        });

        $listItems.detach().appendTo(".list-group");

        isAscending = !isAscending;
        $rankLink.text(isAscending ? "Rank - High to Low" : "Rank - Low to High");
    });

    $(document).on('click', '.edit-rank', function () {
        var $rankInput = $(this).siblings('.rank-input');
        var $rankValue = $(this).siblings('.rank-value');

        $rankValue.hide();
        $rankInput.show();
        $rankInput.focus();
    });

    // Handle update of rank value
    $(document).on('focusout', '.rank-input', function () {
        var $rankInput = $(this);
        var newRank = $rankInput.val();
        var todoItemId = $rankInput.data('todo-item-id');
        var $rankValue = $rankInput.siblings('.rank-value');
        $rankValue.show();
        $rankInput.hide();

        if (newRank !== $rankValue.text())
         updateRankValue(todoItemId, newRank, $rankValue);
    });

    function updateRankValue(todoItemId, newRank, $rankValue) {

        const requestObject = [
            {
                "op": "replace",
                "path": "/rank",
                "value": newRank
            }
        ];   
       
        var apiUrl = pathToDoItemUrl + "/" + todoItemId;
        fetch(apiUrl, {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(requestObject)
        }).then(response => {
            if (response.ok) {
                $rankValue.text(newRank);
                    console.log('Rank updated successfully');
                } else {
                    throw new Error('Error updating rank');
                }
            })
            .catch(error => {
                console.error('Error updating rank:', error);
            });
    }
    

});