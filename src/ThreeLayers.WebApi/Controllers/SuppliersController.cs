// using Microsoft.AspNetCore.Mvc;
// using ThreeLayers.Business.Interfaces;
// using ThreeLayers.Business.Models;
// using ThreeLayers.Contracts;
//
// namespace ThreeLayers.WebApi.Controllers
// {
// 	public class SuppliersController : MyControllerBase
// 	{
// 		private readonly ISupplierRepository _supplierRepository;
// 		private readonly ISupplierService _supplierService;
//
// 		public SuppliersController(ISupplierRepository supplierRepository,
// 			ISupplierService supplierService,
// 			INotifier notifier) : base(notifier)
// 		{
// 			_supplierRepository = supplierRepository;
// 			_supplierService = supplierService;
// 		}
//
// 		[HttpGet]
// 		public async Task<IEnumerable<SupplierResponse>> GetAll()
// 		{
// 			return (await _supplierRepository.GetAllAsync()).Select(x => new SupplierResponse()
// 			{
// 				Active = x.Active,
// 				Document = x.Document,
// 				Id = x.Id,
// 				Name = x.Name
// 			});
// 		}
//
// 		[HttpGet("{id:guid}")]
// 		public async Task<ActionResult<SupplierResponse>> GetById(Guid id)
// 		{
// 			Supplier? supplier = await _supplierRepository.GetByIdAsync(id);
//
// 			if (supplier == null)
// 				return NotFound();
//
// 			return new SupplierResponse(supplier);
// 		}
//
// 		[HttpPost]
// 		public async Task<IEnumerable<SupplierResponse>> Add(SupplierResponse supplier)
// 		{
// 			throw new NotImplementedException();
// 		}
//
// 		[HttpPut]
// 		public async Task<ActionResult<SupplierResponse>> Update(Guid id, SupplierResponse supplier)
// 		{
// 			throw new NotImplementedException();
// 		}
//
// 		[HttpDelete("{id:guid}")]
// 		public async Task<ActionResult<SupplierResponse>> Delete(Guid id)
// 		{
// 			throw new NotImplementedException();
// 		}
//
// 		internal static Supplier ToSupplier(SupplierRequest supplierRequest)
// 		{
// 			return new Supplier()
// 			{
// 				Active = supplierRequest.Active,
// 				Document = supplierRequest.Document,
// 				Name = supplierRequest.Name,
// 				Id = supplierRequest.Id
// 			};
// 		}
// 	}
// }
