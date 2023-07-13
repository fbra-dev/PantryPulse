import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {catchError, first} from 'rxjs/operators';

import { AccountService, AlertService } from '@app/authorization/_services';
import {HttpErrorResponse} from "@angular/common/http";
import {throwError} from "rxjs";

@Component({ templateUrl: 'register.component.html' })
export class RegisterComponent implements OnInit {
    form!: FormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private accountService: AccountService,
        private alertService: AlertService
    ) { }

    ngOnInit() {
        this.form = this.formBuilder.group({
            firstName: ['', Validators.required],
            lastName: ['', Validators.required],
            username: ['', Validators.required],
            password: ['', [Validators.required, Validators.minLength(6)]]
        });
    }

    // convenience getter for easy access to form fields
    get f() { return this.form.controls; }

    onSubmit() {
        this.submitted = true;

        // reset alerts on submit
        this.alertService.clear();

        // stop here if form is invalid
        if (this.form.invalid) {
            return;
        }

        this.loading = true;
        this.accountService.register(this.form.value)
            .pipe(first())
            // .pipe(catchError(this.handleError))
            .subscribe({
                next: () => {
                    this.alertService.success('Registration successful', { keepAfterRouteChange: true });
                    this.router.navigate(['../login'], { relativeTo: this.route });
                },
                error: error => {
                  let errorMessage = error.message;
                  if(error.error)
                  {
                    let jsonString = JSON.stringify(error);
                    let errorCast : HttpErrorResponse = error;
                    errorMessage = "";
                    if (errorCast.error && Array.isArray(errorCast.error[''])) {
                      for (const errMsg of errorCast.error['']) {
                        errorMessage = errorMessage.concat(errMsg.toString());
                      }
                    }
                  }
                  this.alertService.error(errorMessage);
                    this.loading = false;
                }
            });
    }
}
