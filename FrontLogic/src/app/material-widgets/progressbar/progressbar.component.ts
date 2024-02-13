import { Component, OnInit } from '@angular/core';
import {PROGRESSBAR_HELPERS } from './helpers.data';

@Component({
  selector: 'cdk-progressbar',
  templateUrl: './progressbar.component.html',
  styleUrls: ['./progressbar.component.scss']
})
export class ProgressbarComponent implements OnInit {

  public colors: any;
  public modes: any;
  public values: any;
  public bufferValues: any;

  public color: any;
  public mode: any;
  public value: any;
  public bufferValue: any;

  progressbarHelpers: any = PROGRESSBAR_HELPERS;

  constructor() { }

  ngOnInit() {
  }

  
}
