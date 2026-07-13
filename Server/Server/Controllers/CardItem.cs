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
    public class CardItem: Controller
    {
        private readonly ICardItems _cardItems;
        private readonly ICards _card;
        private readonly ILogger<CardItem> _logger;
 
        public CardItem(ICards card, ICardItems cardItems, ILogger<CardItem> logger, ICards cardService)
        {
            _cardItems = cardItems;
            _logger = logger;
            _card = cardService;
        }

        [HttpPatch("Change")]
        public async Task<IActionResult> Change(CardItemRequest cardItemRequest)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                bool res = await _cardItems.Change(cardItemRequest, id);

                return res ? Ok() : BadRequest(new { error = "Bad request" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while changing the card item.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpPatch("IsCompleted")]
        public async Task<IActionResult> IsCompleted(CardItemRequest cardItemRequest)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                bool res = await _cardItems.IsCompleted(cardItemRequest, id);

                return res ? Ok() : BadRequest(new { error = "Bad request" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while changing the card item.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CardItemRequest cardItemRequest)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var res = await _cardItems.Create(cardItemRequest, id);

                if (res == null) return BadRequest(new { error = "Bad request" });

                return Ok(new { item = res });
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "An error occurred while creating the card item.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(CardItemRequest cardItemRequest)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var res = await _cardItems.Delete(cardItemRequest, id);

                return res ? Ok() : BadRequest(new { error = "Bad request" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the card item.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> GetItems(int size, int from, string cardID)
        {
            try
            {
                var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var items = await _cardItems.GetItemsAsync(size, from, id, cardID);

                if (items == null) return BadRequest();

                return Ok(new { items = items });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving card items.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An error occurred while processing your request." });
            }
        }

    }
}
