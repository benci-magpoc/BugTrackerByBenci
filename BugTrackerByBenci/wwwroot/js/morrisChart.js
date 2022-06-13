var morrisDonutData = [
    {
        label: "Low",
        value: @Model.Tickets.Where(t => t.TicketPriority.Name == nameof(BTTicketPriorities.Low)).Count()
            }, {
    label: "Medium",
        value: @Model.Tickets.Where(t => t.TicketPriority.Name == nameof(BTTicketPriorities.Medium)).Count()
}, {
    label: "High",
        value: @Model.Tickets.Where(t => t.TicketPriority.Name == nameof(BTTicketPriorities.High)).Count()
}, {
    label: "Urgent",
        value: @Model.Tickets.Where(t => t.TicketPriority.Name == nameof(BTTicketPriorities.Urgent)).Count()
}
        ];
/*
Chart for Ticket by Priority
*/
if ($('#morrisTicketPriority').get(0)) {
    var donutChart = Morris.Donut({
        resize: true,
        element: 'morrisTicketPriority',
        data: morrisDonutData,
        colors: ['#0088cc', '#734ba9', '#E36159', '#ff993b']
    });

    donutChart.options.data.forEach(function (label, i) {
        var legendItem = $('<span></span>').text(label['label'] + ": " + label['value']).prepend('<span>&nbsp;</span>');
        legendItem.find('span')
            .css('backgroundColor', donutChart.options.colors[i])
            .css('width', '20px')
            .css('display', 'inline-block')
            .css('margin', '10px');
        $('#legend').append(legendItem);
    });
};

//Ticket Status
var morrisDataTicketStatus = [
    {
        label: "New",
        value: @Model.Tickets.Where(t => t.TicketStatus.Name == nameof(BTTicketStatus.New)).Count()
            }, {
    label: "Dev",
        value: @Model.Tickets.Where(t => t.TicketStatus.Name == nameof(BTTicketStatus.Development)).Count()
}, {
    label: "Test",
        value: @Model.Tickets.Where(t => t.TicketStatus.Name == nameof(BTTicketStatus.Testing)).Count()
}, {
    label: "Resolved",
        value: @Model.Tickets.Where(t => t.TicketStatus.Name == nameof(BTTicketStatus.Resolved)).Count()
}
        ];


/*
Chart for Ticket By Status
*/
if ($('#morrisTicketStatus').get(0)) {
    var donutChartStatus = Morris.Donut({
        resize: true,
        element: 'morrisTicketStatus',
        data: morrisDataTicketStatus,
        colors: ['#0088cc', '#734ba9', '#E36159', '#ff993b']
    });

    donutChartStatus.options.data.forEach(function (label, i) {
        var legendItem = $('<span></span>').text(label['label'] + ": " + label['value']).prepend('<span>&nbsp;</span>');
        legendItem.find('span')
            .css('backgroundColor', donutChart.options.colors[i])
            .css('width', '20px')
            .css('display', 'inline-block')
            .css('margin', '10px');
        $('#statusLegend').append(legendItem);
    });
};

var morrisDataTicketType = [
    {
        label: "New Development",
        value: @Model.Tickets.Where(t => t.TicketType.Name == nameof(BTTicketTypes.NewDevelopment)).Count()
            }, {
    label: "Routine",
        value: @Model.Tickets.Where(t => t.TicketType.Name == nameof(BTTicketTypes.WorkTask)).Count()
}, {
    label: "Defect",
        value: @Model.Tickets.Where(t => t.TicketType.Name == nameof(BTTicketTypes.Defect)).Count()
}, {
    label: "Change",
        value: @Model.Tickets.Where(t => t.TicketType.Name == nameof(BTTicketTypes.ChangeRequest)).Count()
}, {
    label: "Enchancement",
        value: @Model.Tickets.Where(t => t.TicketType.Name == nameof(BTTicketTypes.Enhancement)).Count()
}, {
    label: "General",
        value: @Model.Tickets.Where(t => t.TicketType.Name == nameof(BTTicketTypes.GeneralTask)).Count()
}
        ];
/*
Chart for Ticket by Type
*/
if ($('#morrisTicketType').get(0)) {
    var donutChartType = Morris.Donut({
        resize: true,
        element: 'morrisTicketType',
        data: morrisDataTicketType,
        colors: ['#0088cc', '#734ba9', '#E36159', '#ff993b', '#ffc36d', '#7460ee']
    });

    donutChartType.options.data.forEach(function (label, i) {
        var legendItem = $('<span></span>').text(label['label'] + ": " + label['value']).prepend('<span>&nbsp;</span>');
        legendItem.find('span')
            .css('backgroundColor', donutChartType.options.colors[i])
            .css('width', '20px')
            .css('display', 'inline-block')
            .css('margin', '10px');
        $('#typeLegend').append(legendItem);
    });
};

var morrisDataActiveWork = [
    {
        label: "Active Projects",
        value: @Model.Projects.Where(p => p.StartDate < DateTime.Now && p.EndDate > DateTime.Now).Count()
            }, {
    label: "Assigned Tickets",
        value: @Model.Tickets.Where(t => t.DeveloperUser != null).Count()
}, {
    label: "Unassigned Tickets",
        value: @Model.Tickets.Where(t => t.DeveloperUser == null).Count()
}, {
    label: "Members",
        value: @Model.Company.Members.Count()
}
        ];
/*
Chart for Ticket by Type
*/
if ($('#morrisActiveWork').get(0)) {
    var donutChartActiveWork = Morris.Donut({
        resize: true,
        element: 'morrisActiveWork',
        data: morrisDataActiveWork,
        colors: ['#0088cc', '#734ba9', '#E36159', '#ff993b']
    });

    donutChartActiveWork.options.data.forEach(function (label, i) {
        var legendItem = $('<span></span>').text(label['label'] + ": " + label['value']).prepend('<span>&nbsp;</span>');
        legendItem.find('span')
            .css('backgroundColor', donutChartActiveWork.options.colors[i])
            .css('width', '20px')
            .css('display', 'inline-block')
            .css('margin', '10px');
        $('#activeWorkLegend').append(legendItem);
    });
};