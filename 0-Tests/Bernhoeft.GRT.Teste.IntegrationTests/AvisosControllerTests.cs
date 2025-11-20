using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Responses.Queries.v1;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Bernhoeft.GRT.Teste.IntegrationTests
{
    public class AvisosControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AvisosControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAvisos_deve_retornar_seed_inicial()
        {
            // act
            var response = await _client.GetAsync("/api/v1/avisos");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var dataElement = doc.RootElement.GetProperty("Dados");
            var avisos = JsonSerializer.Deserialize<IEnumerable<GetAvisosResponse>>(dataElement.GetRawText());

            avisos.Should().NotBeNull();
            avisos.Should().NotBeEmpty();
            avisos.Should().HaveCountGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task GetAvisoPorId_deve_retornar_aviso_existente()
        {
            // act
            var getByIdResponse = await _client.GetAsync($"/api/v1/avisos/1");

            // assert
            getByIdResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getByIdJson = await getByIdResponse.Content.ReadAsStringAsync();
            using var getByIdDoc = JsonDocument.Parse(getByIdJson);
            var getByIdDataElement = getByIdDoc.RootElement.GetProperty("Dados");
            var avisoRetornado = JsonSerializer.Deserialize<GetAvisosResponse>(getByIdDataElement.GetRawText());

            avisoRetornado.Should().NotBeNull();
            avisoRetornado.Id.Should().Be(1);
            avisoRetornado.Titulo.Should().NotBeNullOrEmpty();
            avisoRetornado.Mensagem.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task PostAviso_deve_criar_e_aparecer_no_Get()
        {
            // arrange
            var request = new CreateAvisoRequest
            {
                Titulo = "Aviso Integração",
                Mensagem = "Mensagem teste"
            };

            // act - POST
            var postResponse = await _client.PostAsJsonAsync("/api/v1/avisos", request);

            // assert POST
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // act - GET
            var getResponse = await _client.GetAsync("/api/v1/avisos");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = await getResponse.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var dataElement = doc.RootElement.GetProperty("Dados");
            var avisos = JsonSerializer.Deserialize<IEnumerable<GetAvisosResponse>>(dataElement.GetRawText());

            avisos.Should().NotBeEmpty();
            avisos.Should().Contain(a => a.Titulo == request.Titulo && a.Mensagem == request.Mensagem);
        }

        [Fact]
        public async Task PutAviso_deve_atualizar_mensagem()
        {
            // arrange
            var createRequest = new CreateAvisoRequest
            {
                Titulo = "Aviso para Atualizar", Mensagem = "Mensagem Original"
            };
            var postResponse = await _client.PostAsJsonAsync("/api/v1/avisos", createRequest);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var getResponse = await _client.GetAsync("/api/v1/avisos");
            var json = await getResponse.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var dataElement = doc.RootElement.GetProperty("Dados");
            var avisos = JsonSerializer.Deserialize<IEnumerable<GetAvisosResponse>>(dataElement.GetRawText());
            var avisoCriado = avisos.First(a => a.Titulo == createRequest.Titulo);

            // act
            var updateBody = new { Mensagem = "Mensagem Atualizada" };
            var putResponse = await _client.PutAsJsonAsync($"/api/v1/avisos/{avisoCriado.Id}", updateBody);

            // assert PUT
            putResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getUpdatedResponse = await _client.GetAsync("/api/v1/avisos");
            var updatedJson = await getUpdatedResponse.Content.ReadAsStringAsync();
            using var updatedDoc = JsonDocument.Parse(updatedJson);
            var updatedDataElement = updatedDoc.RootElement.GetProperty("Dados");
            var updatedAvisos =
                JsonSerializer.Deserialize<IEnumerable<GetAvisosResponse>>(updatedDataElement.GetRawText());
            var avisoAtualizado = updatedAvisos.First(a => a.Id == avisoCriado.Id);

            avisoAtualizado.Mensagem.Should().Be("Mensagem Atualizada");
        }

        [Fact]
        public async Task DeleteAviso_deve_remover_aviso()
        {
            // arrange
            var createRequest = new CreateAvisoRequest
            {
                Titulo = "Aviso para Deletar", Mensagem = "Mensagem a ser deletada"
            };
            var postResponse = await _client.PostAsJsonAsync("/api/v1/avisos", createRequest);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var getResponse = await _client.GetAsync("/api/v1/avisos");
            var json = await getResponse.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var dataElement = doc.RootElement.GetProperty("Dados");
            var avisos = JsonSerializer.Deserialize<IEnumerable<GetAvisosResponse>>(dataElement.GetRawText());
            var avisoCriado = avisos.First(a => a.Titulo == createRequest.Titulo);

            // act
            var deleteResponse = await _client.DeleteAsync($"/api/v1/avisos/{avisoCriado.Id}");

            // assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getAfterDeleteResponse = await _client.GetAsync("/api/v1/avisos");
            var afterDeleteJson = await getAfterDeleteResponse.Content.ReadAsStringAsync();
            using var afterDeleteDoc = JsonDocument.Parse(afterDeleteJson);
            var afterDeleteDataElement = afterDeleteDoc.RootElement.GetProperty("Dados");
            var afterDeleteAvisos =
                JsonSerializer.Deserialize<IEnumerable<GetAvisosResponse>>(afterDeleteDataElement.GetRawText());

            afterDeleteAvisos.Should().NotContain(a => a.Id == avisoCriado.Id);
            afterDeleteAvisos.Should().NotContain(a => a.Titulo == createRequest.Titulo);
        }
    }
}