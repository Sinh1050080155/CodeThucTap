using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test2.Models.Entities;
using test2.Models.Mapping;
using test2.Services.HangHoaServices;

namespace test2.Controllers
{
    [Route("[controller]")]
    public class HH_DM_HangHoaController : Controller
    {
        DemoNhaKhoaContext _context;
        IMapper _mapper;
        IHhDmHangHoaServices _services;
        public HH_DM_HangHoaController(DemoNhaKhoaContext context, IMapper mapper, IHhDmHangHoaServices services)
        {
            _context = context;
            _mapper = mapper;
            _services = services;
        }
        [HttpPost("Modify")]
        public async Task<dynamic> Modify([FromBody] PhieuNhap data)
        {
            return await _services.Modify(data.PhieuNhapMap, data.PhieuNhapCT);
        }
        [HttpPost("Read")]
        public async Task<dynamic> Read()
        {
            return await _services.Read();
        }

    }
    public class PhieuNhap
    {
       public HhPhieuNhapMap PhieuNhapMap { get; set; }
       public List<HhChiTietPhieuNhapMap> PhieuNhapCT { get; set; }
    }
    
}
