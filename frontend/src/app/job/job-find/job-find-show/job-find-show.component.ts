import { Component, OnInit, Input } from '@angular/core';
import { Job } from '../../job.model';
import { Subscription } from 'rxjs';
import { JobService } from '../../job.service';
import { ActivatedRoute, Params } from '@angular/router';
import { AuthService } from 'src/app/shared/auth.service';

@Component({
  selector: 'app-job-find-show',
  templateUrl: './job-find-show.component.html',
  styleUrls: ['./job-find-show.component.css']
})
export class JobFindShowComponent implements OnInit {
  @Input() jobInput: Job;
  editJob = false
  isFetching = false;
  isError = false;
  error = '';
  oneJob: Job;
  jobs: Job[] = [];
  job: string;
  id = '';
  subscriptions: Subscription[] = [];

  constructor(
    private jobServ: JobService,
    private route: ActivatedRoute,
    public auth: AuthService
  ) { }

  ngOnInit() {
    this.jobServ.jobChanged.subscribe(()=>{
      if (!this.jobInput){
        this.getJob();
      }
    }); 
    if (this.jobInput){
      this.job = ""+this.jobInput.id;
      this.oneJob = this.jobInput;
    } else {
      this.subscriptions.push( 
        this.route.params.subscribe((params: Params) =>{
          this.oneJob = null;
          this.jobs = [];
          this.job = params['job'];
          this.getJob();
        })
      );
    }
  }
  
  onDelete(job){
    if (confirm("Are you sure you want to delete job # " +job+ "?")){
      this.jobServ.deleteJob(job).subscribe(()=>{
        this.jobServ.jobChanged.next();
      });
    }
  }

  getJob() {
    this.editJob = false;
    this.isFetching = true;
    if (this.job.includes("part")){
      this.jobServ.fetchJobByPart(this.job)
      .subscribe(jobs => {
        this.jobs = jobs;
        this.isFetching = false;
      }, error => {
        this.isFetching = false;
        this.isError = true;
        this.error = error.message
      });
    } else {
      this.jobServ.fetchJob(this.job)
      .subscribe(job => {
        this.oneJob = job;
        this.isFetching = false;
      }, error => {
        this.isFetching = false;
        this.isError = true;
        this.error = error.message
      });
    }
  } 

  onEdit(){
    this.editJob = true;
  }
  
}