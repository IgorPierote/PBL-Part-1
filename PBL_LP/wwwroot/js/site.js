$(document).ready(function () {
    console.log('JavaScript carregado corretamente');

    $('#filterButton').click(function () {
        console.log('Botão de filtro clicado');

        var cpfFilter = $('#cpfFilter').val();
        var nomeFilter = $('#nomeFilter').val();
        var telefoneFilter = $('#telefoneFilter').val();
        var dataNascimentoFilter = $('#dataNascimentoFilter').val();

        console.log('Filtros capturados:', cpfFilter, nomeFilter, telefoneFilter, dataNascimentoFilter);

        $.ajax({
            url: '@urlFiltrar',
            type: 'GET',
            data: {
                cpfFilter: cpfFilter,
                nomeFilter: nomeFilter,
                telefoneFilter: telefoneFilter,
                dataNascimentoFilter: dataNascimentoFilter
            },
            success: function (result) {
                console.log('Dados recebidos:', result);
                $('#userList').html(result);
            },
            error: function (xhr, status, error) {
                console.error("Erro ao processar a requisição: ", status, error);
            }
        });
    });
});