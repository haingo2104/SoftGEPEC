"use client"

import { useState } from "react";
import SideBar from "../sidebar/page";

async function getDepartments() {
    const res = await fetch(`http://localhost:5236/api/departments`, {
        cache: 'no-store'
    });
    return res.json();
}

export default async function Page() {
        const data = await getDepartments() as Department[];

    return (
        <div> {/* Utiliser une disposition flexible */}
            <SideBar />
            <section className="bg-gray-50 dark:bg-gray-900 py-4 ">
                <div className="flex flex-col sm:ml-64 px-6 md:h-screen lg:py-0">
                    <div>
                        <ul>
                            {data.map((department) => {
                                return (
                                    <li className="dark:text-white" key={department.id}>{department.title}</li>
                                );
                            })}
                        </ul>
                    </div>
                </div>
            </section>
        </div>
    );
}
