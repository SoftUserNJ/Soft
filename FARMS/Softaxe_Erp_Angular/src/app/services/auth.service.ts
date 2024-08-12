import { environment } from './../../environment/environmemt';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
    private http: HttpClient, 
    private dp: DatePipe
    ) {}

  onLogin(obj: any): Observable<any> {
    obj.dtNow = this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss');
    return this.http.post(
      `${environment.apiUrl}/auth/login?username=${obj.username}&password=${obj.password}&dtNow=${obj.dtNow}`,
      obj
    );
  }

  logout(): Observable<any> {
    const dtNow = this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss');
    return this.http.post(`${environment.apiUrl}/auth/logout?dtNow=${dtNow}`, dtNow);
  }

  storeToken(data: any){

    console.log(data)
    localStorage.setItem('token', data.token)
    localStorage.setItem('cmpId', data.cmpId)
    localStorage.setItem('locId', data.locId)
    localStorage.setItem('locName', data.locName)
    localStorage.setItem('superAdmin', data.superAdmin)
    localStorage.setItem('finId', data.finId)
    localStorage.setItem('designation', data.designation)
    localStorage.setItem('userId', data.userId)
    localStorage.setItem('userName', data.userName)
    localStorage.setItem('userType', data.userType)
    localStorage.setItem('dashboard', data.isDashBoard)
    localStorage.setItem('admin', data.admin)
    localStorage.setItem('userImage', data.userImage)
    localStorage.setItem('CmpName', data.cmp.CmpName)
    localStorage.setItem('cmpCont', data.cmp.Contact)
    localStorage.setItem('cmpAdr', data.cmp.CmpAdr)
    localStorage.setItem('Logo', data.cmp.Logo)
    localStorage.setItem('discountSale', data.cmp.DiscountCodeSale)
    localStorage.setItem('otherCreditSale', data.cmp.OtherCreditCodeSale)
    localStorage.setItem('costOfSale', data.cmp.CostofSale.substring(0, 9))
    localStorage.setItem('furtherTax', data.cmp.FurtherTax)
    localStorage.setItem('whFiler', data.cmp.WhFiler)
    localStorage.setItem('whNonFiler', data.cmp.WhNonFiler)
    localStorage.setItem('dayClose', data.cmp.DayClose)
    localStorage.setItem('roundVal', data.cmp.RoundVal)
    localStorage.setItem('reportFormat', data.cmp.ReportFormat)
    localStorage.setItem('approvalSystem', data.cmp.ApprovalSystem)
    localStorage.setItem('locWise', data.cmp.LocationWise);
    localStorage.setItem('poMust', data.cmp.PoMust);
    localStorage.setItem('costCenter', data.cmp.CostCenterControl);
    localStorage.setItem('mobApp', data.cmp.MobApp);
    localStorage.setItem('stopEntry', data.stopEntry);
    localStorage.setItem('distributionPos', data.cmp.DistributionPos);
    localStorage.setItem('inlimit', data.inlimit);
    localStorage.setItem('exportDetail', data.cmp.ExportDetail);
    localStorage.setItem('shiftime', data.shiftime);
    
  
  }

  exportDetail(){
    return localStorage.getItem('exportDetail')
  }

  poMust(){
    return localStorage.getItem('poMust')
  }

  getToken(){
    return localStorage.getItem('token')
  }

  cmpId(){
    return localStorage.getItem('cmpId')
  }

  cmpAdr(){
    return localStorage.getItem('cmpAdr')
  }

  cmpName(){
    return localStorage.getItem('CmpName')
  }

  cmpLogo(){
    return localStorage.getItem('Logo')
  }

  cmpCont(){
    return localStorage.getItem('cmpCont')
  }


  username(){
    return localStorage.getItem('userName')
  }


  locId(){
    return localStorage.getItem('locId')
  }

  finId(){
    return localStorage.getItem('finId')
  }

  cos(){
    return localStorage.getItem('costOfSale')
  }

  ds(){
    return localStorage.getItem('discountSale')
  }

  os(){
    return localStorage.getItem('otherCreditSale')
  }

  dayClose(){
    return localStorage.getItem('dayClose')
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token')
  }

  inlimit(): any {
    return localStorage.getItem('inlimit') ; 
  }

   

  

  
  shiftime(): any {
    return localStorage.getItem('shiftime') ; 
  }


}