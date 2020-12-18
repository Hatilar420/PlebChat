using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace ChatApp.Util{

    public class PaginatedList <T>{
       public List<T> Items{get;set;}
       public int TotalPageCount{get;set;}
       public int currentPage {get;set;}
       public bool NextPage{get;set;}
       public bool PreviousPage{get;set;}

       public PaginatedList(int TotalCount,int CurrentPage,List<T> PaginatedList,int PageCount){
          this.TotalPageCount = TotalCount/PageCount;
          if(TotalCount%PageCount != 0){
              this.TotalPageCount +=1;
          }
          this.currentPage = CurrentPage;
          this.Items = PaginatedList;
          if(TotalPageCount > CurrentPage){
              this.NextPage = true;
          }
          else{
              this.NextPage = false;
          }

          if(CurrentPage > 1){
              this.PreviousPage = true;
          }
          else{
              this.PreviousPage = false;
          }

       }

    }

}