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

            let result = {
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

function getGlobalLivesearchOptions(elem) {
    let route = '/Home/Find';
    let onSelect = (suggestion) => window.location.pathname = `/${suggestion.data.type}/Details/${suggestion.data.id}`;
    let options = getOptions(route, 3, [], onSelect);

    options.groupBy = 'type';
    options.formatGroup = function (suggestion, category) {
        let group = $("<div/>").addClass("autocomplete-group d-flex w-100 justify-content-start");
        let header = $("<strong/>").addClass("h5").text(category == 'Founder' ? 'Учредители' : 'Компании');
        let searchString = elem.val();
        let link = $("<a/>").addClass("ml-2")
            .attr("href", `/${category}/List?search=${searchString}`)
            .text("Продолжить поиск...");

        group.append(header).append(link);
        return group.wrap('<p/>').parent().html();
    }

    return options;
}

if ($('#global-search')) {
    $('#global-search').autocomplete(getGlobalLivesearchOptions($('#global-search')));
}

if ($('#list-search').length) {
    $('#list-search').autocomplete(getListLivesearchOptions($('#list-search')));
}

if ($('#founder-edit').length) {
    let nameElem = $("#Name");
    let taxpayerIdElem = $("#TaxpayerId");
    let founderSearchElem = $("#founder-search");

    function getFounderLivesearchOptions(elem) {
        let onSelect = (suggestion) => {
            let ul = $("#founders>ul.list-group").first();
            $(ul).append(createFounderItem(suggestion.data));

            elem.val("");
            updateAutocompleteOptions();
            setFounderSearchState(getIsIndividualOption() == "true");
        };

        return getOptions('/Founder/Find/', 4, getFoundersId(), onSelect)
    }

    function createFounderItem(founder) {
        let founderItem = $("<li/>").addClass("list-group-item d-flex justify-content-between align-items-center");
        let div = $("<div/>");
        let link = $("<a/>").attr("target", "_blank")
            .attr("href", '/Founder/Details/' + founder.id)
            .attr("data-founder-id", founder.id)
            .attr("data-founder-name", founder.name)
            .attr("data-founder-taxpayer-id", founder.taxpayerId)
            .text(founder.name);

        let muted = $("<small/>").addClass("text-muted ml-2")
            .text("ИНН: " + founder.taxpayerId);

        let badge = $("<a/>").attr("href", "#")
            .addClass("remove-item badge badge-secondary badge-pill")
            .text("✕");
        div.append(link).append(muted);
        founderItem.append(div).append(badge);

        return founderItem;
    }

    function getFoundersId() {
        return getFounders().map((i, e) => parseInt(e.dataset.founderId)).get();
    }

    function getFounders() {
        return $("a[data-founder-id][data-founder-name][data-founder-taxpayer-id]");
    }

    function onTypeChange() {
        let isIndividual = getIsIndividualOption();

        $.each([nameElem, taxpayerIdElem, founderSearchElem], function (index, value) {
            value.parent().toggle(isIndividual != "");
        });

        if (isIndividual != "") {
            setFounderSearchState(isIndividual === 'true')
        }
    }

    function setFounderSearchState(isIndividualFounder) {
        $.each([nameElem, taxpayerIdElem], function (index, elem) {
            setReadonly(elem, isIndividualFounder);
        });

        if (isIndividualFounder) {
            let founders = getFounders();

            if (founders.length > 0) {
                founderSearchElem.autocomplete().disable();
                founderSearchElem.val("");
            }
            else
                founderSearchElem.autocomplete().enable();

            setReadonly(founderSearchElem, founders.length > 0);

            if (founders.length == 1) {
                nameElem.val(`ИП ${founders.first().data('founder-name')}`);
                taxpayerIdElem.val(founders.first().data('founder-taxpayer-id'));
            }
        }
    }

    function getIsIndividualOption() {
        return $("#IsIndividual")
            .children("option:selected")
            .first()
            .val();
    }

    function setReadonly(elem, value) {
        elem.attr('readonly', value);
    }

    function updateAutocompleteOptions() {
        founderSearchElem.autocomplete().setOptions(getFounderLivesearchOptions(founderSearchElem));
        founderSearchElem.autocomplete().clear();
    }

    $(document).ready(onTypeChange);

    $('#founders').on('click', 'a.remove-item', function (e) {
        e.preventDefault();
        $(this).parent("li").remove();

        updateAutocompleteOptions();
        setFounderSearchState(getIsIndividualOption() == "true");
    });

    $("#IsIndividual").change(onTypeChange);

    founderSearchElem.autocomplete(getFounderLivesearchOptions(founderSearchElem));

    $('#founder-edit').submit(function (e) {
        e.preventDefault();

        if ($(this).valid()) {
            $.each(getFoundersId(), function (index, id) {
                let input = $("<input/>")
                    .attr("name", `CompanyFounders[${index}].FounderId`)
                    .attr("type", "hidden")
                    .val(id);
                $('#founder-edit').append(input);
            });

            this.submit();
        }
    });
}