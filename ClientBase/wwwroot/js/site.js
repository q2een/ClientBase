async function getEntities(route, searchQuery) {
    const response = await fetch(route, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        credentials: 'include',
        body: JSON.stringify(searchQuery),
    });

    if (response.ok === true) {
        return await response.json();
    }
}

function getOptions(route, count, exceptIds, onSelect) {
    return {
        preserveInput: true,
        lookup: async function (query, done) {
            if (!query)
                done(undefined);

            let entities = await getEntities(route, {
                text: query,
                count: count, except: exceptIds
            });

            var result = {
                suggestions: $.map(entities, function (entity) {
                    return {
                        value: `${entity.name} (ИНН: ${entity.taxpayerId})`, data: entity};
                })
            };

            done(result);
        },
        onSelect: suggestion => onSelect(suggestion),
    }
}

function getListLivesearchOptions(elem) {
    let route = $(elem).data('livesearch-route');
    let controller = route.split('/')[1];
    let onSelect = (suggestion) => window.location.pathname = `${controller}/Details/${suggestion.data.id}`;

    return getOptions(route, 5, [], onSelect)
}

function getFounderLivesearchOptions(elem) {
    let onSelect = (suggestion) => {
        let id = suggestion.data.id;
        let li = $("<li/>").addClass("list-group-item d-flex justify-content-between align-items-center");
        let div = $("<div/>");
        let link = $("<a/>").attr("target", "_blank")
            .attr("href", '/Founder/Details/' + id)
            .attr("data-founder-id", id)
            .text(suggestion.data.name);

        let muted = $("<small/>").addClass("text-muted ml-2")
            .text("ИНН: " + suggestion.data.taxpayerId);

        let badge = $("<a/>").attr("href", "#")
            .addClass("remove-item badge badge-secondary badge-pill")
            .text("✕");
        div.append(link).append(muted);
        li.append(div).append(badge);

        let ul = $("#founders>ul.list-group")[0];

        $(ul).append(li);

        $(elem).autocomplete().setOptions(getFounderLivesearchOptions(elem));
        $(elem).autocomplete().clear();
    };

    let exceptIds = $("a[data-founder-id]").map(function () {
        return parseInt($(this).data('founder-id'));
    }).get();

    return getOptions('/Founder/Find/', 4, exceptIds, onSelect)
}

if ($('#list-search').length) {
    $('#list-search').autocomplete(getListLivesearchOptions($('#list-search')));
}

if ($('#founder-search').length) {
    $('#founder-search').autocomplete(getFounderLivesearchOptions($('#founder-search')));

    $('#founders').on('click', 'a.remove-item', function (e) {
        e.preventDefault();
        $(this).parent("li").remove();
        $('#founder-search').autocomplete().setOptions(getFounderLivesearchOptions($('#founder-search')));
        $('#founder-search').clear();
    });


    if ($('form#founder-edit').length) {
        $('form#founder-edit').submit(function (e) {
            e.preventDefault();
            var founders = $("a[data-founder-id]");

            $.each(founders, function (index, value) {
                let input = $("<input/>").attr("name", `CompanyFounders[${index}].FounderId`)
                    .attr("type", "hidden")
                    .val($(value).data("founder-id"));
                $(this).append(input);
            });

            if ($(this).valid()) {
                this.submit();
            }
        });
    }
}
