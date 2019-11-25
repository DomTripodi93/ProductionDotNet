import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { Operation } from 'src/app/job/job-ops/operation.model';
import { Machine } from 'src/app/machine/machine.model';
import { OpService } from '../operation.service';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { AuthService } from 'src/app/shared/auth.service';
import { MachineService } from 'src/app/machine/machine.service';

@Component({
  selector: 'app-job-ops-edit',
  templateUrl: './job-ops-edit.component.html',
  styleUrls: ['./job-ops-edit.component.css']
})
export class JobOpsEditComponent implements OnInit {
  @Input() search: string;
  editOpForm: FormGroup;
  operation: Operation;
  canInput = false;
  isError = false;
  error = "";
  machines: Machine[] = [];
  
  constructor(
    private operationServ: OpService,
    private mach: MachineService,
    private auth: AuthService
  ) { }

  ngOnInit() {
    this.search = this.operationServ.slashToDash(this.search);
    this.canInput = this.auth.isAuthenticated;
    this.mach.fetchMachinesByType()
    .subscribe(machines => {
      this.machines = machines;
      this.operationServ.fetchOp(this.search)
      .subscribe(operation => {
        this.operation = operation;
        this.initForm();
      });
    });
  }


  private initForm() {
    let machine = this.operation.machine;
    let cycleTime = this.operation.cycleTime;

    this.editOpForm = new FormGroup({
      'cycleTime': new FormControl(cycleTime),
      'machine': new FormControl(machine)
    });
  }

  onSubmit(){
    this.editOp(this.editOpForm.value);
  }

  editOp(data: Operation) {
    this.isError = false;
    this.operationServ.changeOp(data, this.search).subscribe(()=>{
      setTimeout(
        ()=>{
          this.operationServ.opsChanged.next();
        }, 100
      );
    });
  }

  onCancel(){
    this.operationServ.opsChanged.next();
  }

}
