using Core.Card;
using Core.Models.Request;
using Core.User;
using DALPostgresSQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class Card: Controller
    {
        private readonly ICards _card;
        private readonly ILogger<Card> _logger;
 
        public Card(ICards card, ICardItems cardItems, ILogger<Card> logger)
        {
            _card = card;
            _logger = logger;
        }


        [HttpPatch("Change")]
        public async Task<IActionResult> Change(CardRequest cardRequest)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var res = await _card.Change(cardRequest, id);


                return res ? Ok(res) : BadRequest(new { error = "Bad request" });
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while changing the card.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CardRequest cardRequest)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var res = await _card.Create(cardRequest, id);
                return res != null ? Ok(new {card = res }) : BadRequest(new { error = "Bad request" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the card.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(CardRequest cardRequest)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                bool res = await _card.Delete(cardRequest, id);
                return res ? Ok() : BadRequest(new { error = "Bad request" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the card.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetCards(int size, int from, string? category)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var cards = await _card.GetCardsAsync(size, from, id, category);

                return cards != null ? Ok(new { Cards = cards }) : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the card.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{cardId}")]
        public async Task<IActionResult> GetCardsById(string cardId)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var card = await _card.GetCardById(cardId, id);

                return card != null ? Ok(new { Card = card }) : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the card.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request." });
            }
        }
    }
}
