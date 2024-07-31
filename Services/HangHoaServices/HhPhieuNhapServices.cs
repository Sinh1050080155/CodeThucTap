using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using test2.Models.Entities;
using test2.Models.Mapping;

namespace test2.Services.HangHoaServices
{
    public interface IHhPhieuNhapServices
    {
        Task<dynamic> Modify(HhPhieuNhapMap hhPhieuNhapMap);
        Task<dynamic> Read();
    }

    public class HhPhieuNhapServices : IHhPhieuNhapServices
    {
        private readonly DemoNhaKhoaContext _context;
        private readonly IMapper _mapper;

        public HhPhieuNhapServices(DemoNhaKhoaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<dynamic> Modify(HhPhieuNhapMap hhPhieuNhapMap)
        {
            using var tran = await _context.Database.BeginTransactionAsync();
            try
            {
                HhPhieuNhap master = _mapper.Map<HhPhieuNhap>(hhPhieuNhapMap);
                if (master.Id == 0)
                {
                    await _context.HhPhieuNhaps.AddAsync(master);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    HhPhieuNhap a = await _context.HhPhieuNhaps.FindAsync(master.Id);
                    
                        a.SoPhieuNhap = master.SoPhieuNhap;
                        a.NgayTao = master.NgayTao;
                        a.GhiChu = master.GhiChu;

                        _context.HhPhieuNhaps.Update(a);
                    
                }
                await _context.SaveChangesAsync();
                tran.Commit();

                return new
                {
                    statusCode = 200,
                    message = "Thanh cong",
                    mater = await _context.HhPhieuNhaps.ToListAsync(),
                };
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return new {
                    statusCode = 500,
                    message = "that bai",
                };
            }
        }

        public async Task<dynamic> Read()
        {
            var data = await _context.HhPhieuNhaps
                .Select(x => new
                {
                    Id = x.Id,
                    NgayTao = x.NgayTao,
                    SoPhieuNhap = x.SoPhieuNhap,
                    GhiChu = x.GhiChu,
                })
                .ToListAsync();

            return data;
        }
    }
}
