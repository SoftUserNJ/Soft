import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
declare const $: any;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Assets';

  constructor(private router: Router) {}

  isLoginPage: boolean = true;

  ngOnInit() {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.isLoginPage = event.url === '/login' || event.url === '/';
        if (event.url.includes('/flock-performance') || event.url.includes('/view-report') || event.url.includes('/flock-exp-report')) {
          this.isLoginPage = true;
      }
      }
    });
  }
  
  ngAfterViewInit(){
    if(this.router.url === '/purchase-invoice'){
      $("#toggle_btn").click();
    }
    else{
      $("#toggle_btn").click();
    }
  }


  // node --max-old-space-size=4096 ./node_modules/@angular/cli/bin/ng serve

}