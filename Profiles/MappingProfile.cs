using AutoMapper;
using ProdutosAPi.DTOs;

namespace ProdutosAPi.Profiles 
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    { 
      CreateMap<Produto, ProdutoDto>();
      CreateMap<CriarProdutosDto, Produto>();
      CreateMap<AtualizarProdutosDto, Produto>();
    }
  }
}
