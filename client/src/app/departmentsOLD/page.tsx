import SideBar from "../sidebar/page";

async function getDepartments() {
    const res = await fetch(`http://localhost:5236/api/departments`);
    return res.json();
}

export default async function Page() {
    const data = await getDepartments() as Department[];
    console.log(data);

    return (
        <div className="flex">
            {/* <SideBar /> */}
            <section >
                <ul>
                {data.map((department) => {
                    return (
                        <li key={department.id}>{department.title}</li>
                    );
                })}
            </ul>
            </section>


        </div>
    );
}
