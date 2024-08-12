
import { Component, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-report-modal',
  templateUrl: './report-modal.component.html',
  styleUrls: ['./report-modal.component.css']
})
export class ReportModalComponent implements OnInit {
  @Input() reportUrl: string;

  constructor(public bsModalRef: BsModalRef ) { }

  ngOnInit() {
  
  }
  closeModal() {
    this.bsModalRef.hide();
  }
}
