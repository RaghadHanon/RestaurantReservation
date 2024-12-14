using AuthenticationService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.ModelView.Table;
using RestaurantReservation.API.Utilities;
using RestaurantReservation.Db.Entities;
using RestaurantReservation.Db.Interfaces;

namespace RestaurantReservation.Api.Controllers;

[Authorize]
[Route("api/restaurants/{restaurantId}/tables")]
[ApiController]
public class TablesController : ControllerBase
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly ITableRepository _tablesRepository;
    private readonly IMapper _mapper;

    public TablesController(
        IRestaurantRepository restaurantRepository,
        ITableRepository tablesRepository,
        IMapper mapper)
    {
        _restaurantRepository = restaurantRepository;
        _tablesRepository = tablesRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all tables in a restaurant.
    /// </summary>
    /// <param name="restaurantId">The ID of the restaurant.</param>
    /// <returns>The list of tables in the restaurant.</returns>
    /// <response code="200">Returns the list of tables in the restaurant.</response>
    /// <response code="404">If the restaurant is not found or the table is not found in the restaurant.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TableDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<TableDto>>> GetTables(int restaurantId)
    {
        var restaurantExists = await _restaurantRepository.RestaurantExistsAsync(restaurantId);
        if (!restaurantExists)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        var tables = await _tablesRepository.GetTablesInRestaurantAsync(restaurantId);
        return Ok(_mapper.Map<IEnumerable<TableDto>>(tables));
    }

    /// <summary>
    /// Gets a specific table in a restaurant by ID.
    /// </summary>
    /// <param name="restaurantId">The ID of the restaurant.</param>
    /// <param name="tableId">The ID of the table.</param>
    /// <returns>The table data transfer object.</returns>
    /// <response code="200">Returns the requested table.</response>
    /// <response code="404">If the restaurant is not found or the table is not found in the restaurant.</response>
    [HttpGet("{tableId}", Name = "GetTable")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TableDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TableDto>> GetTable(int restaurantId, int tableId)
    {
        var restaurantExists = await _restaurantRepository.RestaurantExistsAsync(restaurantId);
        if (!restaurantExists)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        var table = await _tablesRepository.GetTableAsync(restaurantId, tableId);
        if (table == null)
        {
            return NotFound(ApiErrors.TableNotFound);
        }

        return Ok(_mapper.Map<TableDto>(table));
    }

    /// <summary>
    /// Creates a new table in a restaurant.
    /// </summary>
    /// <param name="restaurantId">The ID of the restaurant.</param>
    /// <param name="tableForCreation">The data for creating a new table.</param>
    /// <returns>The created table.</returns>
    /// <response code="201">Returns the created table.</response>
    /// <response code="400">If the data is invalid.</response>
    /// <response code="404">If the restaurant is not found or the table is not found in the restaurant.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TableDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostTable(int restaurantId, TableCreationDto tableForCreation)
    {
        var restaurantExists = await _restaurantRepository.RestaurantExistsAsync(restaurantId);
        if (!restaurantExists)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        var tableEntity = _mapper.Map<Table>(tableForCreation);
        var createdTable = await _tablesRepository.CreateTableAsync(restaurantId, tableEntity);
        await _tablesRepository.SaveChangesAsync();

        return CreatedAtRoute("GetTable",
            new {
                restaurantId,
                tableId = createdTable.TableId
            }, _mapper.Map<TableDto>(createdTable));
    }

    /// <summary>
    /// Updates an existing table in a restaurant.
    /// </summary>
    /// <param name="restaurantId">The ID of the restaurant.</param>
    /// <param name="tableId">The ID of the table to update.</param>
    /// <param name="tableForUpdate">The data for updating the table.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">No content if successful.</response>
    /// <response code="400">If the data is invalid.</response>
    /// <response code="404">If the restaurant is not found or the table is not found in the restaurant.</response>
    [HttpPut("{tableId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PutTable(int restaurantId, int tableId, TableUpdateDto tableForUpdate)
    {
        var restaurantExists = await _restaurantRepository.RestaurantExistsAsync(restaurantId);
        if (!restaurantExists)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        var tableEntity = await _tablesRepository.GetTableAsync(restaurantId, tableId);
        if (tableEntity == null)
        {
            return NotFound(ApiErrors.TableNotFound);
        }

        _mapper.Map(tableForUpdate, tableEntity);
        await _tablesRepository.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Deletes an existing table in a restaurant.
    /// </summary>
    /// <param name="restaurantId">The ID of the restaurant.</param>
    /// <param name="tableId">The ID of the table to delete.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">No content if successful.</response>
    /// <response code="400">If the table cannot be deleted.</response>
    /// <response code="404">If the restaurant is not found or the table is not found in the restaurant.</response>
    [Authorize(Roles = UserRoles.Admin)]
    [HttpDelete("{tableId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTable(int restaurantId, int tableId)
    {
        var restaurantExists = await _restaurantRepository.RestaurantExistsAsync(restaurantId);
        if (!restaurantExists)
        {
            return NotFound(ApiErrors.RestaurantNotFound);
        }

        var tableEntity = await _tablesRepository.GetTableAsync(restaurantId, tableId);
        if (tableEntity == null)
        {
            return NotFound(ApiErrors.TableNotFound);
        }

        try
        {
            _tablesRepository.DeleteTable(tableEntity);
            await _tablesRepository.SaveChangesAsync();
        }
        catch (Exception)
        {
            return BadRequest(ApiErrors.DeletionIsInvalid);
        }

        return NoContent();
    }
}