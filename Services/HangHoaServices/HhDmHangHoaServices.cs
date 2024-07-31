using AutoMapper;
using Microsoft.EntityFrameworkCore;
using test2.Models.Entities;
using test2.Models.Mapping;

namespace test2.Services.HangHoaServices
{
    public interface IHhDmHangHoaServices
    {
        Task<dynamic> Modify(HhPhieuNhapMap hhPhieuNhapMap, List<HhChiTietPhieuNhapMap> hhChiTietPhieuNhaps);
        Task<dynamic> Read();
    }
    public class HhDmHangHoaServices : IHhDmHangHoaServices
    {
        DemoNhaKhoaContext _context;
        IMapper _mapper;
        public HhDmHangHoaServices(DemoNhaKhoaContext context, IMapper mapper) 
        {
            _context = context; 
            _mapper = mapper;
        }
        public async Task<dynamic> Modify(HhPhieuNhapMap hhPhieuNhapMap, List<HhChiTietPhieuNhapMap> hhChiTietPhieuNhaps)
        {
            var tran = _context.Database.BeginTransaction();
            try
            {
                List<HhChiTietPhieuNhap> detail = _mapper.Map<List<HhChiTietPhieuNhap>>(hhChiTietPhieuNhaps);
                HhPhieuNhap master = _mapper.Map<HhPhieuNhap>(hhPhieuNhapMap);
                if (master.Id == 0)
                {
                   await _context.HhPhieuNhaps.AddAsync(master);
                   await _context.SaveChangesAsync();
                    foreach (HhChiTietPhieuNhap ct in detail)
                    {
                        ct.IdphieuNhap = master.Id;
                    }
                    await _context.HhChiTietPhieuNhaps.AddRangeAsync(detail);

                }
                else
                {
                    HhPhieuNhap a = await _context.HhPhieuNhaps.FindAsync(master.Id);
                    a.SoPhieuNhap = master.SoPhieuNhap;
                    a.NgayTao = master.NgayTao;
                    a.GhiChu = master.GhiChu;

                    _context.HhPhieuNhaps.Update(a);
                    _context.HhChiTietPhieuNhaps.UpdateRange(detail);   
                }
                await _context.SaveChangesAsync();
                tran.Commit();
                return new
                {
                    statusCode = 200,
                    message = "Thanh cong",
                    mater = await _context.HhPhieuNhaps.ToListAsync(),
                    detail = detail
                };
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return new
                {
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
                    HhChiTietPhieuNhaps = x.HhChiTietPhieuNhaps.Select(y => new 
                    {
                        Id = y.Id,
                        IdhangHoa = y.IdhangHoa,
                        IdhangHoaNavigation = new 
                        {
                            Id = y.IdhangHoaNavigation.Id,
                            TenHangHoa = y.IdhangHoaNavigation.TenHangHoa,
                            IddonViTinhNavigation = y.IdhangHoaNavigation.IddonViTinhNavigation != null ?  new 
                            {
                                Id = y.IdhangHoaNavigation.IddonViTinhNavigation.Id,
                                TenDvt = y.IdhangHoaNavigation.IddonViTinhNavigation.TenDvt
                            } : null
                        }
                    }).ToList()
                })
                .ToListAsync();
            return data;
        }
    }
}
