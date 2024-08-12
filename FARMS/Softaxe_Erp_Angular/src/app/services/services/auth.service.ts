import { environment } from 'src/environment/environmemt';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DatePipe } from '@angular/common';

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
debugger
    localStorage.setItem('token', data.token)
    localStorage.setItem('cmpId', data.cmpId)
    localStorage.setItem('locId', data.locId)
    localStorage.setItem('locName', data.locName)
    localStorage.setItem('superAdmin', data.superAdmin)
    localStorage.setItem('finId', data.finId)
    localStorage.setItem('designation', data.designation)
    localStorage.setItem('userId', data.userId)
    localStorage.setItem('userName', data.userName)
    localStorage.setItem('dashboard', data.isDashBoard)
    localStorage.setItem('admin', data.admin)
    localStorage.setItem('userImage', data.userImage)
    localStorage.setItem('CmpName', data.cmp.CmpName)
    localStorage.setItem('cmpEmail', data.cmp.Email)
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
    localStorage.setItem('locWise', data.cmp.LocationWise)
    localStorage.setItem('Des1', data.Des1)
    localStorage.setItem('Des2', data.Des2)
    localStorage.setItem('Des3', data.Des3)
    localStorage.setItem('Des4', data.Des4)
    localStorage.setItem('Des5', data.Des5)
    localStorage.setItem('Des6', data.Des6)
    localStorage.setItem('DayCloseTime', data.DayCloseTime)
  }

  getToken(){

    return localStorage.getItem('token')
  }

  cmpId(){
    return localStorage.getItem('cmpId')
  }

  cmpName(){
    return localStorage.getItem('CmpName')
  }

  cmpLogo(){
    return localStorage.getItem('Logo')
  }

  cmpEmail(){
    return localStorage.getItem('cmpEmail')
  }

  cmpCont(){
    return localStorage.getItem('cmpCont')
  }

  cmpAdr(){ debugger
    return localStorage.getItem('cmpAdr')
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

  DayCloseTime(){
    return localStorage.getItem('DayCloseTime')
  }
  
}