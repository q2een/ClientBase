async function getEntities(route, object, action, errorHandler) {
    const response = await fetch(route, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        credentials: 'include',
        body: JSON.stringify(object),
    });

    if (response.ok === true) {
        const entities = await response.json();
        action(entities);
    }
    else {
        errorHandler(response.status);
    }
}

$("#search").keyup(async function (e) {
    if (e.which <= 90 && e.which >= 48) {
    livesearch($("#search"), [], 3)

    }
});

async function livesearch(elem, exceptIds, count) {
    let search = elem.val();
    let dropdownMenu = elem.siblings(".dropdown-menu");
    dropdownMenu.empty();

    if (search.length == 0) {
        $(dropdownMenu).dropdown("hide");
        return;
    }

    const entities = await getEntities(elem.data('livesearch-route'), {
        text: search,
        except: exceptIds,
        count: count
    }, function (entities) {

            if (entities.length == 0) {
                $(dropdownMenu).dropdown("hide");

            return;
            }
            appendLivesearh(entities);

            $(dropdownMenu).dropdown("show");
    });
}

function appendLivesearh(entities) {
    //let menu = $("<div/>").attr("aria-labelledby", "search")
    //    .addClass("dropdown-menu");

    let menu = $("#search").siblings(".dropdown-menu").first();

    $.each(entities, (index, value) => {
        let link = $("<a/>").attr("href", "/Details/" + value.id)
            .addClass("dropdown-item")
            .data("entity-name", value.name)
            .data("entity-id", value.id)
            .data("entity-taxpayer-id", value.taxpayerId)
            .text(value.name);

        let muted = $("<small/>").addClass("text-muted ml-2")
            .text("ИНН: " + value.taxpayerId);

        link.append(muted);
        menu.append(link);
    });

    //$("#search").append(menu);
}