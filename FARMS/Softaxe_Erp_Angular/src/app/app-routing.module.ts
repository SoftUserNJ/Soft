import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './features/layout/dashboard/dashboard.component';
import { ViewReportComponent } from './features/reports/view-report/view-report.component';
import { LoginComponent } from './features/auth/login/login.component';
import { authGuard } from './guards/auth.guard';
import { ViewInvoiceOnAppComponent } from './common/view-invoice-on-app/view-invoice-on-app.component';
import { UserProfileComponent } from './features/auth/user-profile/user-profile.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'report', component: ViewReportComponent },
  { path: 'view-report', component: ViewInvoiceOnAppComponent },
  {
    path: 'user-profile',
    component: UserProfileComponent,
    canActivate: [authGuard],
  },

  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authGuard],
  },
  {
    path: 'accounts',
    loadChildren: () =>
      import('./features/accounts/accounts.module').then(
        (m) => m.AccountsModule
      ),
  },
  {
    path: 'inventory',
    loadChildren: () =>
      import('./features/inventory/inventory.module').then(
        (m) => m.InventoryModule
      ),
  },
  {
    path: 'production',
    loadChildren: () =>
      import('./features/production/production.module').then(
        (m) => m.ProductionModule

      ),
  },
  {
    path: 'GeneralStore',
    loadChildren: () =>
      import('./features/general-store/general-store.module').then(
        (m) => m.GeneralStoreModule
      ),
  },
  {
    path: 'sale',
    loadChildren: () =>
      import('./features/sale/sale.module').then((m) => m.SaleModule),
  },
  {
    path: 'purchase',
    loadChildren: () =>
      import('./features/purchase/purchase.module').then(
        (m) => m.PurchaseModule
      ),
  },
  {
    path: 'utilities',
    loadChildren: () =>
      import('./features/utilities/utilities.module').then(
        (m) => m.UtilitiesModule
      ),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule],
})
export class AppRoutingModule {}
