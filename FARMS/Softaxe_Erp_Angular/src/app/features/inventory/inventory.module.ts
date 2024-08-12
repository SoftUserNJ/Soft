import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ProductsComponent } from './products/products.component';
import { StockOpeningEntryComponent } from './stock-opening-entry/stock-opening-entry.component';
import { StockStatusComponent } from './stock-status/stock-status.component';
import { WearhouseComponent } from './wearhouse/wearhouse.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { ReactiveFormsModule } from '@angular/forms';
import { StockDebitNoteComponent } from './stock-debit-note/stock-debit-note.component';
import { StockCreditNoteComponent } from './stock-credit-note/stock-credit-note.component';
import { StockTransferComponent } from './stock-transfer/stock-transfer.component';
import { ChangeCategoryComponent } from './change-category/change-category.component';
import { authGuard } from 'src/app/guards/auth.guard';
import { InventoryListComponent } from './inventory-list/inventory-list.component';
import { ProductLedgerComponent } from './product-ledger/product-ledger.component';
import { PurchaseRateComparisonComponent } from './purchase-rate-comparison/purchase-rate-comparison.component';
import { ProductRateComparisonComponent } from './product-rate-comparison/product-rate-comparison.component';
import { CalendarModule } from 'primeng/calendar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatListModule } from '@angular/material/list';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { ProductRateUpdateComponent } from './product-rate-update/product-rate-update.component';
import { ProductsErpComponent } from './products-erp/products-erp.component';
import { MaterialConsumptionComponent } from './material-consumption/material-consumption.component';

@NgModule({
  declarations: [
    ProductsComponent,
    StockOpeningEntryComponent,
    StockStatusComponent,
    WearhouseComponent,
    StockDebitNoteComponent,
    StockCreditNoteComponent,
    StockTransferComponent,
    ChangeCategoryComponent,
    InventoryListComponent,
    ProductLedgerComponent,
    PurchaseRateComparisonComponent,
    ProductRateComparisonComponent,
    ProductRateUpdateComponent,
    ProductsErpComponent,
    MaterialConsumptionComponent,
  ],
  imports: [  
    CommonModule,
    RouterModule.forChild([
      { path: 'wearhouse', component: WearhouseComponent, canActivate: [authGuard] },
      { path: 'products', component: ProductsComponent, canActivate: [authGuard] },
      { path: 'products-erp', component: ProductsErpComponent, canActivate: [authGuard] },
      { path: 'products-rate-update', component: ProductRateUpdateComponent, canActivate: [authGuard] },
      { path: 'change-category', component: ChangeCategoryComponent, canActivate: [authGuard] },
      { path: 'stock-opening-entry', component: StockOpeningEntryComponent, canActivate: [authGuard] },
      { path: 'stock-debit-note', component: StockDebitNoteComponent, canActivate: [authGuard] },
      { path: 'stock-credit-note', component: StockCreditNoteComponent, canActivate: [authGuard] },
      { path: 'stock-transfer', component: StockTransferComponent, canActivate: [authGuard] },
      { path: 'stock-status', component: StockStatusComponent, canActivate: [authGuard] },
      { path: 'product-rate-comparison', component: ProductRateComparisonComponent, canActivate: [authGuard] },
      { path: 'purchase-rate-comparison', component: PurchaseRateComparisonComponent, canActivate: [authGuard] },
      { path: 'inventory-list', component: InventoryListComponent, canActivate: [authGuard] },
      { path: 'product-ledger', component: ProductLedgerComponent, canActivate: [authGuard] },
      { path: 'material-consumption', component: MaterialConsumptionComponent, canActivate: [authGuard] },
    ]),
    NgSelectModule,
    RouterModule,
    FormsModule,
    MatButtonModule,
    MatInputModule,
    MatSelectModule,
    MatToolbarModule,
    MatListModule,
    MatFormFieldModule,
    MatIconModule,
    MatCardModule ,
    MatAutocompleteModule,
    BrowserModule,
    MatDatepickerModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    CalendarModule,
    DatePipe
  ],
  exports: [
    ProductsComponent,
    StockOpeningEntryComponent,
    StockStatusComponent,
    WearhouseComponent,
    StockDebitNoteComponent,
    StockCreditNoteComponent,
    ChangeCategoryComponent
  ],
})
export class InventoryModule {}