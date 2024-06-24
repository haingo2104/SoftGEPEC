import SideBar from "../sidebar/page";

async function getUsers() {
    const res = await fetch(`http://localhost:5236/api/users`);
    return res.json();
}

export default async function Page() {
    const data = await getUsers() as Users[];
    console.log(data);

    return (
        <div className="flex">
            {/* <SideBar /> */}
            <section >
                <ul>
                {data.map((user) => {
                    return (
                        <li key={user.id}>{user.id}</li>
                    );
                })}
            </ul>
            </section>


        </div>
    );
}
