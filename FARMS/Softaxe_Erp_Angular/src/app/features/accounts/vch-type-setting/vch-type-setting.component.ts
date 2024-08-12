import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-vch-type-setting',
  templateUrl: './vch-type-setting.component.html',
  styleUrls: ['./vch-type-setting.component.css'],
})
export class VchTypeSettingComponent {
  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private tostr: ToastrService
  ) {}

  voucherTypeForm!: FormGroup;
  vchList: any[] = [];

  ngOnInit(): void {
    this.getVchTypes();
    this.formInit();
  }

  formInit() {
    this.voucherTypeForm = this.fb.group({
      id: [undefined],
      name: [''],
      description: [''],
      verifyName: [''],
      verifyRequired: false,
      approvalName: [''],
      approvalRequired: false,
      auditName: [''],
      lastText: [''],
    });
  }

  getVchTypes() {
    this.apiService.getData('Accounts/GetVchTypes').subscribe((data) => {
      this.vchList = data;
    });
  }

  onChangeVch(event: any) {
    this.voucherTypeForm.get('id')?.patchValue(event.id);
    this.voucherTypeForm.get('name')?.patchValue(event.vchtype);
    this.voucherTypeForm.get('description')?.patchValue(event.Description);
    this.voucherTypeForm.get('verifyName')?.patchValue(event.VerifyName);
    this.voucherTypeForm.get('verifyRequired')?.patchValue(event.VerifyRequired);
    this.voucherTypeForm.get('approvalName')?.patchValue(event.ApprovalName);
    this.voucherTypeForm.get('approvalRequired')?.patchValue(event.ApprovalRequired);
    this.voucherTypeForm.get('auditName')?.patchValue(event.AuditName);
    this.voucherTypeForm.get('lastText')?.patchValue(event.LASTtext);
  }

  onClearVch() {
    this.voucherTypeForm.reset();
  }

  onClickSave() {
    const body = this.voucherTypeForm.value;
    if (body.id == undefined || body.id == 0) {
      this.tostr.warning('Select Vch Type...!');
      return;
    }

    this.apiService
      .saveObj('Accounts/GetVchTypesDataUpdate', body)
      .subscribe((data) => {
        if (data == true || data == 'true') {
          this.tostr.success('Save Successfully');
          this.getVchTypes();
          this.onClearVch();
        } else {
          this.tostr.error('Please Save Again');
        }
      });
  }
}