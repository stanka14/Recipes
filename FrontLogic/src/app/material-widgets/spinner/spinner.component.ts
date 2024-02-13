import { Component, OnInit } from '@angular/core';
import { SPINNER_HELPERS } from './helpers.data';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';

@Component({
  selector: 'cdk-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.scss']
})
export class SpinnerComponent implements OnInit {

  public showSource: any;
  public colors = 'primary';
  public modes: ProgressSpinnerMode = 'determinate';
  public values = 50;

  public color: any;
  public mode: any;
  public value: any;

  public spinnerHelpers: any = SPINNER_HELPERS;
  constructor() { }

  ngOnInit() {
  }
  // showProgressBarCode;
	  
}
